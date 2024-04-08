using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.OrderDetails;
using PTP.Application.ViewModels.Orders;
using PTP.Domain.Enums;

namespace PTP.Application.Features.Orders.Queries;

public class GetAllOrderQuery : IRequest<OrderBasicModel>
{
    public Guid StoreId { get; set; }
    public class QueryValidation : AbstractValidator<GetAllOrderQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }

    public class QueryHandler : IRequestHandler<GetAllOrderQuery, OrderBasicModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private ILogger<QueryHandler> _logger;

        public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<QueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }
        public async Task<OrderBasicModel> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.OrderRepository.WhereAsync(x => x.StoreId == request.StoreId);
            if (orders.Count == 0) throw new NotFoundException($"No Orders with StoreId-{request.StoreId} is not founded!");
            var result = new OrderBasicModel
            {
                StoreId = request.StoreId,
                OrderWaiting = orders.Count(x => x.Status == nameof(OrderStatusEnum.Waiting)),
                OrderPreparing = orders.Count(x => x.Status == nameof(OrderStatusEnum.Preparing)),
                OrderPrepared = orders.Count(x => x.Status == nameof(OrderStatusEnum.Prepared)),
                OrderCompleted = orders.Count(x => x.Status == nameof(OrderStatusEnum.Completed)),
                OrderCanceled = orders.Count(x => x.Status == nameof(OrderStatusEnum.Canceled)),
                Total = orders.Count()
            };
            return result;
        }
    }
}