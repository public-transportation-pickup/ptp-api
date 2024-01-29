using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.OrderDetails;
using PTP.Application.ViewModels.Orders;

namespace PTP.Application.Features.Orders.Queries;

public class GetOrdersByIdQuery:IRequest<OrderViewModel>
{
    public Guid Id{get;set;}
    public class QueryValidation : AbstractValidator<GetOrdersByIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }

    public class QueryHandler : IRequestHandler<GetOrdersByIdQuery, OrderViewModel>
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
        public async Task<OrderViewModel> Handle(GetOrdersByIdQuery request, CancellationToken cancellationToken)
        {
            // if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            // var cacheResult = await _cacheService.GetAsync<Category>(CacheKey.CATE+request.Id);
            // if (cacheResult is not null)
            // {
            //     return _mapper.Map<CategoryViewModel>(cacheResult);
            // }
            
            var order= await _unitOfWork.OrderRepository.FirstOrDefaultAsync(x=>
                        x.Id==request.Id,
                        x=>x.Store,x=>x.Station,x=>x.Payment);
            if(order is null) throw new NotFoundException($"Order with {request.Id} is not existed!");
            var result= _mapper.Map<OrderViewModel>(order);
            var ordetails= await _unitOfWork.OrderDetailRepository.WhereAsync(x=>x.OrderId==request.Id,x=>x.ProductInMenu,x=>x.ProductInMenu.Product);
            result.OrderDetails= _mapper.Map<IEnumerable<OrderDetailViewModel>>(ordetails);
            return result;
        }
    }
}