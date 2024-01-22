using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.OrderDetails;
using PTP.Application.ViewModels.Orders;
using PTP.Application.ViewModels.Payments;
using PTP.Domain.Entities;
using PTP.Domain.Enums;

namespace PTP.Application.Features.Orders;

public class CreateOrderCommand:IRequest<OrderViewModel>
{
    public OrderCreateModel CreateModel{get;set;}=default!;

    public class CommmandValidation : AbstractValidator<CreateOrderCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.CreateModel.PhoneNumber).NotNull().NotEmpty().WithMessage("PhoneNumber must not null or empty");
            RuleFor(x => x.CreateModel.Total).NotNull().NotEmpty().WithMessage("Total must not null or empty");
            RuleFor(x => x.CreateModel.UserId).NotNull().NotEmpty().WithMessage("UserId must not null or empty");
            RuleFor(x => x.CreateModel.StationId).NotNull().NotEmpty().WithMessage("StationId must not null or empty");
            RuleFor(x => x.CreateModel.StoreId).NotNull().NotEmpty().WithMessage("StoreId must not null or empty");
            RuleFor(x => x.CreateModel.Payment.Total).NotNull().NotEmpty().WithMessage("Payment Total must not null or empty");
            RuleFor(x => x.CreateModel.Payment.PaymentType).NotNull().NotEmpty().WithMessage("PaymentType Total must not null or empty");
            RuleFor(x => x.CreateModel.OrderDetails).NotNull().NotEmpty().WithMessage("OrderDetails must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<CreateOrderCommand, OrderViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        private readonly ICacheService _cacheService;
        public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CommandHandler> logger,
                ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
            _logger=logger;
            _cacheService=cacheService;
        }
        public async Task<OrderViewModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Create Order:\n");
            var order= _mapper.Map<Order>(request.CreateModel);
            await CreateOrderDetail(order.Id, request.CreateModel.OrderDetails);
            order.PaymentId = await CreatePayment(order.Id, request.CreateModel.Payment);
            await CreateTransaction(order);
            await _unitOfWork.OrderRepository.AddAsync(order);
            if(!await _unitOfWork.SaveChangesAsync()) throw new BadRequestException("SaveChanges Fail!");
            return _mapper.Map<OrderViewModel>(order);
        }

        private async Task CreateOrderDetail(Guid orderId,List<OrderDetailCreateModel> models)
        {
            var orderDetails= _mapper.Map<List<OrderDetail>>(models);
            for (int i = 0; i < orderDetails.Count; i++)
            {
                orderDetails[i].OrderId=orderId;
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
            var userWallet= await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x=>x.UserId==order.UserId);
            if(userWallet!.Amount<order.Total) throw new BadRequestException("Wallet is not enough monney!");
            var storeWallet =  await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x=>x.StoreId==order.StoreId);
            if(storeWallet is null ) throw new BadRequestException($"Wallet have Store Id-{order.StoreId} does not exist!");

            var transactions= new List<Transaction>{
                new Transaction{Name=nameof(TransactionTypeEnum.Transfer),Amount=order.Total,TransactionType=nameof(TransactionTypeEnum.Transfer),WalletId=userWallet.Id,PaymentId=order.PaymentId},
                new Transaction{Name=nameof(TransactionTypeEnum.Receive),Amount=order.Total,TransactionType=nameof(TransactionTypeEnum.Receive),WalletId=storeWallet.Id,PaymentId=order.PaymentId}
            };
            await _unitOfWork.TransactionRepository.AddRangeAsync(transactions);
            userWallet.Amount  -= order.Total;
            storeWallet.Amount += order.Total;
            _unitOfWork.WalletRepository.UpdateRange(new List<Wallet>{userWallet,storeWallet});
        }
    }
}