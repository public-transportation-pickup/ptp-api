using AutoMapper;
using Azure.Core;
using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Orders;
using PTP.Domain.Entities;
using PTP.Domain.Enums;

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
            private ILogger<CommandHandler> _logger;
            private readonly IClaimsService _claimService;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ICacheService cacheService,
                    ILogger<CommandHandler> logger,
                    IMapper mapper,
                    IBackgroundJobClient backgroundJob,
                    IClaimsService claimsService)
            {
                _unitOfWork = unitOfWork;
                _cacheService = cacheService;
                _logger = logger;
                _mapper = mapper;
                _backgroundJob = backgroundJob;
                _claimService = claimsService;
            }

            public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update Order:\n");

                var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.UpdateModel.Id);
                if (order is null) throw new NotFoundException($"Order with Id-{request.UpdateModel.Id} is not exist!");

                var status = request.UpdateModel.Status;
                switch (status)
                {
                    case nameof(OrderStatusEnum.Preparing):
                        order = await PreparingState(order);
                        break;
                    case nameof(OrderStatusEnum.Prepared):
                        order = PreparedState(order);
                        break;
                    case nameof(OrderStatusEnum.Completed):
                        order = CompletedState(order);
                        break;
                    case nameof(OrderStatusEnum.Canceled):
                        if (order.Status == "Waiting" || order.Status == "Preparing") await CancelOrder(order, request.UpdateModel.CanceledReason!);
                        break;

                }


                _unitOfWork.OrderRepository.Update(order);
                var result = await _unitOfWork.SaveChangesAsync();
                return result;
            }
            private async Task<Order> PreparingState(Order order)
            {
                if (!order.Status.Equals("Waiting"))
                    throw new BadRequestException("Order can update to preparing when status is Preparing!");
                var orderCheck = await _unitOfWork.OrderRepository.WhereAsync(x => x.Status == nameof(OrderStatusEnum.Preparing));
                var menu = await _unitOfWork.MenuRepository.GetByIdAsync(order!.MenuId);
                if (orderCheck.Count < menu!.MaxNumOrderProcess) throw new BadRequestException("Số lượng đơn hàng đã vượt quá giới hạn!");
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
                if (!order.Status.Equals("Waiting") && !order.Status.Equals("Preparing"))
                    throw new BadRequestException("Order can cancel when status is Waiting or Preparing!");
                order.Status = nameof(OrderStatusEnum.Canceled);
                order.CanceledReason = reason;
                await CreateTransaction(order);
                return order;
            }
            private async Task CreateTransaction(Order order)
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
        }
    }

}
