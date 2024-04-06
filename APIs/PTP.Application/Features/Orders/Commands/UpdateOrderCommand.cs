using AutoMapper;
using Azure.Core;
using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Orders;
using PTP.Domain.Entities;
using PTP.Domain.Enums;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Orders.Commands
{
    public class UpdateOrderCommand : IRequest<bool>
    {
        public OrderUpdateModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateOrderCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.UpdateModel.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
                RuleFor(x => x.UpdateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateOrderCommand, bool>
        {
            private readonly IBackgroundJobClient _backgroundJob;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICacheService _cacheService;
            private readonly IMapper _mapper;
            private readonly AppSettings _app;
            private readonly IClaimsService _claimService;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ICacheService cacheService,
                    AppSettings app,
                    IMapper mapper,
                    IBackgroundJobClient backgroundJob,
                    IClaimsService claimsService)
            {
                _unitOfWork = unitOfWork;
                _cacheService = cacheService;
                _app = app;
                _mapper = mapper;
                _backgroundJob = backgroundJob;
                _claimService = claimsService;
            }

            public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                string title = "";
                string body = "";
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.UpdateModel.Id, x => x.OrderDetails, x => x.Store, x => x.User);
                if (order is null) throw new NotFoundException($"Order with Id-{request.UpdateModel.Id} is not exist!");
                var status = request.UpdateModel.Status;
                switch (status)
                {
                    case nameof(OrderStatusEnum.Preparing):
                        order = PreparingState(order);
                        title = "Đơn hàng đã được xác nhận";
                        body = $"Cửa hàng {order.Store.Name} đã xác nhận đơn hàng của bạn!";
                        break;
                    case nameof(OrderStatusEnum.Prepared):
                        order = PreparedState(order);
                        title = "Đơn hàng đã được chuẩn bị";
                        body = $"Đơn hàng tại {order.Store.Name} đã được chuẩn bị! Vui lòng đến lấy  trước khi hết hạn";
                        break;
                    case nameof(OrderStatusEnum.Completed):
                        order = CompletedState(order);
                        title = "Đơn hàng đã được giao";
                        body = $"{order.Store.Name}! Cảm ơn đã mua hàng!";
                        break;
                    case nameof(OrderStatusEnum.Canceled):
                        if (order.Status == "Waiting" || order.Status == "Preparing")
                        {
                            await CancelOrder(order, request.UpdateModel.CanceledReason!);
                            await UpdateQuantity(order);
                            title = "Đơn hàng đã bị hủy";
                            body = $"Đơn hàng của bạn đã hủy thành công! Vui lòng xác nhận lại";
                        }
                        break;

                }


                _unitOfWork.OrderRepository.Update(order);
                var result = await _unitOfWork.SaveChangesAsync();
                // var tmp = "f7XpUca9TNSSn3cnH9Ecc_:APA91bEgcpDsepM3k8q_dlee-b47Lywrv8KxgjXBzxTWwJVc4m46UTpQIVip9Wc7gPLc-LAoxNlSmYLmomOgY_TiVwZkuujbWcw5FR6J7Qw8iqH5cMSNvl5u6o85LjnEHqkSBN9Y1wyw";
                await FirebaseUtilities.SendNotification(order.User!.FCMToken!, title, body, _app.FirebaseSettings.SenderId, _app.FirebaseSettings.ServerKey);
                // await FirebaseUtilities.SendNotification(tmp, title, body, _app.FirebaseSettings.SenderId, _app.FirebaseSettings.ServerKey);
                return result;
            }

            private Order PreparingState(Order order)
            {
                if (!order.Status.Equals("Waiting"))
                    throw new BadRequestException("Order can update to preparing when status is Preparing!");
                // var orderCheck = await _unitOfWork.OrderRepository.WhereAsync(x => x.Status == nameof(OrderStatusEnum.Preparing));
                // var menu = await _unitOfWork.MenuRepository.GetByIdAsync(order!.MenuId);
                // if (orderCheck.Count < menu!.MaxNumOrderProcess) throw new BadRequestException("Số lượng đơn hàng đã vượt quá giới hạn!");
                order.Status = nameof(OrderStatusEnum.Preparing);
                return order;
            }


            private Order PreparedState(Order order)
            {
                if (!order.Status.Equals("Preparing"))
                    throw new BadRequestException("Order can update to preparing when status is Waiting!");
                order.Status = nameof(OrderStatusEnum.Prepared);
                return order;
            }

            private Order CompletedState(Order order)
            {
                if (!order.Status.Equals("Prepared"))
                    throw new BadRequestException("Order can update to preparing when status is Prepared!");
                order.Status = nameof(OrderStatusEnum.Completed);
                return order;
            }

            private async Task<Order> CancelOrder(Order order, string reason)
            {
                decimal percent = (decimal)0.7;
                if (!order.Status.Equals("Waiting") && !order.Status.Equals("Preparing"))
                    throw new BadRequestException("Order can cancel when status is Waiting or Preparing!");
                order.CanceledReason = reason;
                order.ReturnAmount = order.Status.Equals(nameof(OrderStatusEnum.Waiting)) ? order.Total : order.Total * percent;
                order.Status = nameof(OrderStatusEnum.Canceled);
                await CreateTransaction(order);
                return order;
            }

            private async Task UpdateQuantity(Order order)
            {
                var orderDetails = order.OrderDetails;
                var products = new List<Product>();
                var productMenus = new List<ProductInMenu>();
                foreach (var item in orderDetails)
                {
                    var productMenu = await _unitOfWork.ProductInMenuRepository.FirstOrDefaultAsync(x => x.Id == item.ProductMenuId);
                    var product = await _unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == productMenu!.ProductId);
                    productMenu!.QuantityUsed -= item.Quantity;
                    if (productMenu!.Status == ProductInMenuStatusEnum.InActive.ToString() && productMenu.QuantityInDay > productMenu.QuantityUsed)
                    {
                        productMenu.Status = ProductInMenuStatusEnum.Active.ToString();
                        product!.Status = ProductInMenuStatusEnum.Active.ToString();
                    }
                    productMenus.Add(productMenu);
                    products.Add(product!);
                }
                _unitOfWork.ProductInMenuRepository.UpdateRange(productMenus);
                _unitOfWork.ProductRepository.UpdateRange(products);
                if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
                await _cacheService.RemoveByPrefixAsync(CacheKey.PRODUCTMENU);
                await _cacheService.RemoveByPrefixAsync(CacheKey.PRODUCT);
            }
            private async Task CreateTransaction(Order order)
            {
                var userWallet = await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x => x.UserId == order.UserId);
                if (userWallet is null) throw new BadRequestException($"Wallet have User Id-{order.UserId} does not exist!");
                var storeUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.StoreId == order.StoreId, x => x.Wallet);
                if (storeUser is null) throw new BadRequestException($"Wallet have Store Id-{order.StoreId} does not exist!");
                if (storeUser!.Wallet.Amount < order.Total) throw new BadRequestException("Wallet is not enough monney!");
                var transactions = new List<Transaction>{
                    new Transaction{Name=nameof(TransactionTypeEnum.Receive),Amount=order.ReturnAmount!.Value,TransactionType=nameof(TransactionTypeEnum.Receive),WalletId=userWallet.Id,PaymentId=order.PaymentId},
                    new Transaction{Name=nameof(TransactionTypeEnum.Transfer),Amount=order.ReturnAmount!.Value,TransactionType=nameof(TransactionTypeEnum.Transfer),WalletId=storeUser.WalletId,PaymentId=order.PaymentId}
                };
                await _unitOfWork.TransactionRepository.AddRangeAsync(transactions);
                userWallet.Amount += order.ReturnAmount!.Value;
                storeUser.Wallet.Amount -= order.ReturnAmount.Value!;
                _unitOfWork.WalletRepository.UpdateRange(new List<Wallet> { userWallet, storeUser.Wallet });
            }
        }
    }

}
