using AutoMapper;
using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.OrderDetails;
using PTP.Application.ViewModels.Orders;
using PTP.Application.ViewModels.Payments;
using PTP.Domain.Entities;
using PTP.Domain.Enums;
using System.Globalization;

namespace PTP.Application.Features.Orders;

public class CreateOrderCommand : IRequest<OrderViewModel>
{
    public OrderCreateModel CreateModel { get; set; } = default!;

    public class CommmandValidation : AbstractValidator<CreateOrderCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.CreateModel.PhoneNumber).NotNull().NotEmpty().WithMessage("PhoneNumber must not null or empty");
            RuleFor(x => x.CreateModel.Total).NotNull().NotEmpty().WithMessage("Total must not null or empty");
            RuleFor(x => x.CreateModel.PickUpTime)
                .GreaterThanOrEqualTo(DateTime.Now)
                .NotNull().NotEmpty().WithMessage("PickUpTime must not null or empty");
            //RuleFor(x => x.CreateModel.UserId).NotNull().NotEmpty().WithMessage("UserId must not null or empty");
            RuleFor(x => x.CreateModel.StationId).NotNull().NotEmpty().WithMessage("StationId must not null or empty");
            RuleFor(x => x.CreateModel.StoreId).NotNull().NotEmpty().WithMessage("StoreId must not null or empty");
            RuleFor(x => x.CreateModel.Payment.Total).NotNull().NotEmpty().WithMessage("Payment Total must not null or empty");
            RuleFor(x => x.CreateModel.Payment.PaymentType).NotNull().NotEmpty().WithMessage("PaymentType Total must not null or empty");
            RuleFor(x => x.CreateModel.OrderDetails).NotNull().NotEmpty().WithMessage("OrderDetails must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<CreateOrderCommand, OrderViewModel>
    {
        private readonly IBackgroundJobClient _backgroundJob;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        private readonly IClaimsService _claimsService;
        private readonly ICacheService _cacheService;
        public CommandHandler(IUnitOfWork unitOfWork,
                            IMapper mapper,
                            ILogger<CommandHandler> logger,
                            IClaimsService claimsService,
                            ICacheService cacheService,
                            IBackgroundJobClient backgroundJob)
        {
            _backgroundJob = backgroundJob;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _claimsService = claimsService;
            _cacheService = cacheService;
        }



        public async Task<OrderViewModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create Order:\n");
            var order = _mapper.Map<Order>(request.CreateModel);

            var productInMenus = await GetProductInMenus(request.CreateModel.OrderDetails);
            order.TotalPreparationTime = GetTotalPreparationTime(productInMenus, request.CreateModel.OrderDetails);
            if (DateTime.Now.AddMinutes(order.TotalPreparationTime) > order.PickUpTime.AddHours(1))
                throw new BadRequestException($"Prepration time is not valid - {order.TotalPreparationTime}");

            productInMenus = CheckProductInStock(productInMenus, request.CreateModel.OrderDetails);

            order.UserId = _claimsService.GetCurrentUser;
            await CreateOrderDetail(order.Id, request.CreateModel.OrderDetails);
            order.PaymentId = await CreatePayment(order.Id, request.CreateModel.Payment);
            await CreateTransaction(order);
            await _unitOfWork.OrderRepository.AddAsync(order);
            _unitOfWork.ProductInMenuRepository.UpdateRange(productInMenus);
            if (!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("SaveChanges Fail!");
            AutoApprove(order);
            return _mapper.Map<OrderViewModel>(order);
        }


        private async Task<List<ProductInMenu>> GetProductInMenus(List<OrderDetailCreateModel> models)
        {
            var ids = models.Select(x => x.ProductMenuId);
            return await _unitOfWork.ProductInMenuRepository.WhereAsync(x => ids.Contains(x.Id), x => x.Product);
        }

        private int GetTotalPreparationTime(List<ProductInMenu> productInMenus, List<OrderDetailCreateModel> models)
        {
            productInMenus = productInMenus.OrderBy(x => x.Id).ToList();
            models = models.OrderBy(x => x.ProductMenuId).ToList();
            var total = 0;
            for (int i = 0; i < models.Count; i++)
            {
                var tmp = (double)models[i].Quantity / (double)productInMenus[i].NumProcessParallel;
                total += (int)Math.Ceiling(tmp) * productInMenus[i].PreparationTime;
            }

            return total;
        }

        private List<ProductInMenu> CheckProductInStock(List<ProductInMenu> productInMenus, List<OrderDetailCreateModel> models)
        {
            productInMenus = productInMenus.OrderBy(x => x.Id).ToList();
            models = models.OrderBy(x => x.ProductMenuId).ToList();
            for (int i = 0; i < models.Count; i++)
            {
                if (models[i].Quantity > (productInMenus[i].QuantityInDay - productInMenus[i].QuantityUsed))
                    throw new BadRequestException($"Product - {productInMenus[i].Product.Name} is not enough in stock!");

                else if ((models[i].Quantity - (productInMenus[i].QuantityInDay - productInMenus[i].QuantityUsed)) == 0)
                {
                    productInMenus[i].QuantityUsed += models[i].Quantity;
                    productInMenus[i].Status = ProductInMenuStatusEnum.INACTIVE.ToString();
                }
                else
                {
                    productInMenus[i].QuantityUsed += models[i].Quantity;
                }
            }
            return productInMenus;
        }

        private void AutoApprove(Order order)
        {
            var orderId = order.Id;
            var timeGap = order.PickUpTime - DateTime.Now;
            var prepareTime = order.TotalPreparationTime;
            if (timeGap.Minutes > 0 && timeGap.Minutes < 10)
            {
                _backgroundJob.Schedule(() => BackgroundJobForConfirm(orderId), TimeSpan.FromMinutes(2));
                _backgroundJob.Schedule(() => BackgroundJob(orderId, OrderStatusEnum.Prepared.ToString(), nameof(OrderStatusEnum.Preparing)), TimeSpan.FromMinutes(2 + prepareTime));
            }
            else if (timeGap.Minutes > 10 && timeGap.Minutes < 30)
            {
                _backgroundJob.Schedule(() => BackgroundJobForConfirm(orderId), TimeSpan.FromMinutes(5));
                _backgroundJob.Schedule(() => BackgroundJob(orderId, OrderStatusEnum.Prepared.ToString(), nameof(OrderStatusEnum.Preparing)), TimeSpan.FromMinutes(5 + prepareTime));

            }
            else
            {
                _backgroundJob.Schedule(() => BackgroundJobForConfirm(orderId), TimeSpan.FromMinutes(10));
                _backgroundJob.Schedule(() => BackgroundJob(orderId, OrderStatusEnum.Prepared.ToString(), nameof(OrderStatusEnum.Preparing)), TimeSpan.FromMinutes(10 + prepareTime));
            }
            _backgroundJob.Schedule(() => BackgroundJob(orderId, OrderStatusEnum.PickUpTimeOut.ToString(), nameof(OrderStatusEnum.Prepared)), TimeSpan.FromMinutes(timeGap.Minutes + 60));
        }

        public async Task BackgroundJob(Guid orderId, string status, string statusCheck)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null) throw new BadRequestException($"Order- {orderId} is not found!");
            if (order.Status == statusCheck)
            {
                order.Status = status;
                _unitOfWork.OrderRepository.Update(order);
                if (!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("SaveChanges Fail!");
            }
        }
        public async Task BackgroundJobForConfirm(Guid orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            var orderCheck = await _unitOfWork.OrderRepository.WhereAsync(x => x.Status == nameof(OrderStatusEnum.Preparing));
            var menu = await _unitOfWork.MenuRepository.GetByIdAsync(order!.MenuId);
            if (order == null) throw new BadRequestException($"Order- {orderId} is not found!");
            if (order.Status == nameof(OrderStatusEnum.Waiting))
            {
                order.Status = orderCheck.Count < menu!.MaxNumOrderProcess ? nameof(OrderStatusEnum.Preparing) : nameof(OrderStatusEnum.StoreCanceled);
                order.CanceledReason = orderCheck.Count < menu!.MaxNumOrderProcess ? null : "Số lượng đơn hàng đã vượt quá giới hạn!";
                if (order.Status == nameof(OrderStatusEnum.StoreCanceled)) await RollBackTransaction(order);
                _unitOfWork.OrderRepository.Update(order);
                if (!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("SaveChanges Fail!");
            }
        }

        private async Task CreateOrderDetail(Guid orderId, List<OrderDetailCreateModel> models)
        {
            var orderDetails = _mapper.Map<List<OrderDetail>>(models);
            for (int i = 0; i < orderDetails.Count; i++)
            {
                orderDetails[i].OrderId = orderId;
            }
            await _unitOfWork.OrderDetailRepository.AddRangeAsync(orderDetails);
        }

        private async Task<Guid> CreatePayment(Guid orderId, PaymentCreateModel model)
        {
            var payment = new Payment { Total = model.Total, PaymentType = model.PaymentType, OrderId = orderId };
            await _unitOfWork.PaymentRepository.AddAsync(payment);
            return payment.Id;
        }

        public async Task RollBackTransaction(Order order)
        {
            var userWallet = await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x => x.UserId == order.UserId);
            if (userWallet!.Amount < order.Total) throw new BadRequestException("Wallet is not enough monney!");
            var storeUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.StoreId == order.StoreId, x => x.Wallet);
            if (storeUser is null) throw new BadRequestException($"Wallet have Store Id-{order.StoreId} does not exist!");

            var transactions = new List<Transaction>{
                    new Transaction{Name=nameof(TransactionTypeEnum.Receive),Amount=order.Total,TransactionType=nameof(TransactionTypeEnum.Receive),WalletId=userWallet.Id,PaymentId=order.PaymentId},
                    new Transaction{Name=nameof(TransactionTypeEnum.Transfer),Amount=order.Total,TransactionType=nameof(TransactionTypeEnum.Transfer),WalletId=storeUser.WalletId,PaymentId=order.PaymentId}
                };
            await _unitOfWork.TransactionRepository.AddRangeAsync(transactions);
            userWallet.Amount += order.Total;
            storeUser.Wallet.Amount -= order.Total;
            _unitOfWork.WalletRepository.UpdateRange(new List<Wallet> { userWallet, storeUser.Wallet });
        }

        private async Task CreateTransaction(Order order)
        {
            var userWallet = await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x => x.UserId == order.UserId);
            if (userWallet!.Amount < order.Total) throw new BadRequestException("Wallet is not enough monney!");
            var userStore = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.StoreId == order.StoreId, x => x.Wallet);

            if (userStore!.Wallet is null) throw new BadRequestException($"Wallet have Store Id-{order.StoreId} does not exist!");

            var transactions = new List<Transaction>{
                new Transaction{Name=nameof(TransactionTypeEnum.Transfer),Amount=order.Total,TransactionType=nameof(TransactionTypeEnum.Transfer),WalletId=userWallet.Id,PaymentId=order.PaymentId},
                new Transaction{Name=nameof(TransactionTypeEnum.Receive),Amount=order.Total,TransactionType=nameof(TransactionTypeEnum.Receive),WalletId=userStore.WalletId,PaymentId=order.PaymentId}
            };
            await _unitOfWork.TransactionRepository.AddRangeAsync(transactions);
            userWallet.Amount -= order.Total;
            userStore.Wallet.Amount += order.Total;
            _unitOfWork.WalletRepository.UpdateRange(new List<Wallet> { userWallet, userStore.Wallet });
        }
    }
}