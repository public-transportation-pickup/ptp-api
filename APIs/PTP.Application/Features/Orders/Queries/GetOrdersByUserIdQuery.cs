using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Orders;

namespace PTP.Application.Features.Orders.Queries;

public class GetOrdersByUserIdQuery:IRequest<PaginatedList<OrderViewModel>>
{
    public Guid UserId{get;set;}
    public Dictionary<string, string>? Filter { get; set; } = default!;
    public int PageNumber{get;set;}
    public int PageSize{get;set;}
    public class QueryValidation : AbstractValidator<GetOrdersByUserIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }

    public class QueryHandler : IRequestHandler<GetOrdersByUserIdQuery, PaginatedList<OrderViewModel>>
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
        public async Task<PaginatedList<OrderViewModel>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            // if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            // var cacheResult = await _cacheService.GetAsync<Category>(CacheKey.CATE+request.Id);
            // if (cacheResult is not null)
            // {
            //     return _mapper.Map<CategoryViewModel>(cacheResult);
            // }
            request.Filter!.Remove("pageSize");
            request.Filter!.Remove("pageNumber");
            var orders= await _unitOfWork.OrderRepository.WhereAsync(x=>
                        x.UserId==request.UserId,
                        x=>x.Store,x=>x.Station,x=>x.Payment);
            if(orders.Count==0) throw new NotFoundException("There are no order existed!");
            var viewModels= _mapper.Map<IEnumerable<OrderViewModel>>(orders);
            
            var filterResult = request.Filter.Count > 0 ? new List<OrderViewModel>() : viewModels;

            if(request.Filter!.Count>0)
            {
                foreach(var filter in request.Filter) 
                {
                    filterResult=filterResult.Union(FilterUtilities.SelectItems(viewModels, filter.Key, filter.Value));
                }
            }
            filterResult= filterResult.OrderBy(o => Math.Abs((o.PickUpTime - DateTime.Now).TotalSeconds)).ToList();

            return PaginatedList<OrderViewModel>.Create(
                        source:filterResult.AsQueryable(),
                        pageIndex:request.PageNumber,
                        pageSize:request.PageSize
                );
        }
    }
}