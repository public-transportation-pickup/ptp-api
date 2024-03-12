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
            await CheckMenu(request.CreateModel.MenuId);

            var productInMenus = await CheckProductInStock(request.CreateModel.OrderDetails);

            var order = _mapper.Map<Order>(request.CreateModel);
            order.UserId = _claimsService.GetCurrentUser;
            await CreateOrderDetail(order.Id, request.CreateModel.OrderDetails);
            order.PaymentId = await CreatePayment(order.Id, request.CreateModel.Payment);
            await CreateTransaction(order);
            await _unitOfWork.OrderRepository.AddAsync(order);
            _unitOfWork.ProductInMenuRepository.UpdateRange(productInMenus);
            if (!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("SaveChanges Fail!");
            AutoApprove(order.Id, order.PickUpTime);
            return _mapper.Map<OrderViewModel>(order);
        }

        private async Task CheckMenu(Guid menuId)
        {
            var menu = await _unitOfWork.MenuRepository.GetByIdAsync(menuId);
            var quantityAfterCreate = menu!.NumOrderSold + 1;
            if (quantityAfterCreate > menu.NumOrderEstimated)
                throw new BadRequestException("The quantity allowed in the menu has been exceeded!");
            else if (menu!.NumOrderEstimated == quantityAfterCreate)
            {
                menu.NumOrderSold = quantityAfterCreate;
                menu.Status = MenuEnum.INACTIVE.ToString();
            }
            else
            {
                menu.NumOrderSold = quantityAfterCreate;
            }
            _unitOfWork.MenuRepository.Update(menu);
        }

        private async Task<List<ProductInMenu>> CheckProductInStock(List<OrderDetailCreateModel> models)
        {
            var ids = models.Select(x => x.ProductMenuId);
            var productInMenus = await _unitOfWork.ProductInMenuRepository.WhereAsync(x => ids.Contains(x.Id), x => x.Product);
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

        private void AutoApprove(Guid orderId, DateTime pickUpTime)

        {
            var timeGap = pickUpTime - DateTime.Now;
            if (timeGap.Minutes > 0 && timeGap.Minutes < 10)
            {
                _backgroundJob.Schedule(() => BackgroundJob(orderId), TimeSpan.FromMinutes(2));
            }
            else if (timeGap.Minutes > 10 && timeGap.Minutes < 30)
            {
                _backgroundJob.Schedule(() => BackgroundJob(orderId), TimeSpan.FromMinutes(5));
            }
            else
            {
                _backgroundJob.Schedule(() => BackgroundJob(orderId), TimeSpan.FromMinutes(10));
            }

        }

        private async Task BackgroundJob(Guid orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null) throw new BadRequestException($"Order- {orderId} is not found!");
            if (order.Status == nameof(OrderStatusEnum.Waiting))
            {
                order.Status = OrderStatusEnum.Preparing.ToString();
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