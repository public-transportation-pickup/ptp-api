using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Features.Categories.Commands;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Orders;
using PTP.Domain.Entities;
using PTP.Domain.Enums;
using PTP.Domain.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICacheService _cacheService;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ICacheService cacheService,
                    ILogger<CommandHandler> logger,
                    IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _cacheService = cacheService;
                _logger = logger;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update Order:\n");
                //Remove From Cache       

                var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.UpdateModel.Id);
                if (order is null) throw new NotFoundException($"Order with Id-{request.UpdateModel.Id} is not exist!");

                if (request.UpdateModel.Status.Equals(nameof(OrderStatusEnum.Canceled)))
                {
                    await CancelOrder(order);
                }
                order = _mapper.Map(request.UpdateModel, order);

                _unitOfWork.OrderRepository.Update(order);
                var result = await _unitOfWork.SaveChangesAsync();
                return result;
            }

            private async Task CancelOrder(Order order)
            {
                if (!order.Status.Equals("Payed") || !order.Status.Equals("Confirmed"))
                    throw new BadRequestException("Order can be cancel when status is Payed or Confirmed");
                await CreateTransaction(order);
            }
            private async Task CreateTransaction(Order order)
            {
                var userWallet = await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x => x.UserId == order.UserId);
                if (userWallet!.Amount < order.Total) throw new BadRequestException("Wallet is not enough monney!");
                var storeUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.StoreId == order.StoreId, x => x.Wallet);
                if (storeUser is null) throw new BadRequestException($"Wallet have Store Id-{order.StoreId} does not exist!");

                var transactions = new List<Transaction>{
                    new Transaction{Name=nameof(TransactionTypeEnum.Receive),Amount=order.Total,TransactionType=nameof(TransactionTypeEnum.Transfer),WalletId=userWallet.Id,PaymentId=order.PaymentId},
                    new Transaction{Name=nameof(TransactionTypeEnum.Transfer),Amount=order.Total,TransactionType=nameof(TransactionTypeEnum.Receive),WalletId=storeUser.WalletId,PaymentId=order.PaymentId}
                };
                await _unitOfWork.TransactionRepository.AddRangeAsync(transactions);
                userWallet.Amount += order.Total;
                storeUser.Wallet.Amount -= order.Total;
                _unitOfWork.WalletRepository.UpdateRange(new List<Wallet> { userWallet, storeUser.Wallet });
            }
        }
    }

}
