using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Orders;

namespace PTP.Application.Features.Orders.Queries;

public class GetOrdersByUserIdQuery:IRequest<IEnumerable<OrderViewModel>>
{
    public Guid UserId{get;set;}
    public class QueryValidation : AbstractValidator<GetOrdersByUserIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }

    public class QueryHandler : IRequestHandler<GetOrdersByUserIdQuery, IEnumerable<OrderViewModel>>
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
        public async Task<IEnumerable<OrderViewModel>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            // if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            // var cacheResult = await _cacheService.GetAsync<Category>(CacheKey.CATE+request.Id);
            // if (cacheResult is not null)
            // {
            //     return _mapper.Map<CategoryViewModel>(cacheResult);
            // }
            
            var orders= await _unitOfWork.OrderRepository.WhereAsync(x=>
                        x.StoreId==request.UserId,
                        x=>x.Store,x=>x.Station,x=>x.Payment);
            if(orders.Count==0) throw new NotFoundException("There are no order existed!");
            return _mapper.Map<IEnumerable<OrderViewModel>>(orders);
        }
    }
}