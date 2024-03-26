using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.OrderDetails;
using PTP.Application.ViewModels.Orders;
using PTP.Domain.Entities;
using PTP.Domain.Enums;

namespace PTP.Application.Features.Orders.Queries;

public class GetOrdersByStoreIdQuery : IRequest<Pagination<OrderViewModel>>
{
    public Guid StoreId { get; set; }
    public Dictionary<string, string>? Filter { get; set; } = default!;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? RoleName { get; set; }
    public class QueryValidation : AbstractValidator<GetOrdersByStoreIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }

    public class QueryHandler : IRequestHandler<GetOrdersByStoreIdQuery, Pagination<OrderViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private ILogger<QueryHandler> _logger;
        private readonly IClaimsService _claimsService;

        public QueryHandler(IUnitOfWork unitOfWork,
                            IMapper mapper,
                            ICacheService cacheService,
                            ILogger<QueryHandler> logger,
                            IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
            _claimsService = claimsService;
        }
        public async Task<Pagination<OrderViewModel>> Handle(GetOrdersByStoreIdQuery request, CancellationToken cancellationToken)
        {

            request.Filter!.Remove("pageSize");
            request.Filter!.Remove("pageNumber");
            var orders = await GetOrders(request.StoreId, request.RoleName!);
            var viewModels = _mapper.Map<IEnumerable<OrderViewModel>>(orders);
            viewModels = await GetOrderDetail(viewModels.ToList());
            var filterResult = request.Filter.Count > 0 ? new List<OrderViewModel>() : viewModels;

            if (request.Filter!.Count > 0)
            {
                foreach (var filter in request.Filter)
                {
                    filterResult = filterResult.Union(FilterUtilities.SelectItems(viewModels, filter.Key, filter.Value));
                }
            }
            filterResult = filterResult.OrderBy(o => Math.Abs((o.PickUpTime - DateTime.Now).TotalSeconds)).ToList();


            return new Pagination<OrderViewModel>
            {
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                TotalItemsCount = filterResult.Count(),
                Items = PaginatedList<OrderViewModel>.Create(
                        source: filterResult.AsQueryable(),
                        pageIndex: request.PageNumber,
                        pageSize: request.PageSize
                )
            };
        }

        private async Task<List<Order>> GetOrders(Guid storeId, string role)
        {
            var orders = new List<Order>();
            if (role == nameof(RoleEnum.StoreManager))
            {
                orders = await _unitOfWork.OrderRepository.WhereAsync(x =>
                       x.StoreId == storeId &&
                       x.ModificatedBy == _claimsService.GetCurrentUser,
                       x => x.Store, x => x.Station, x => x.Payment, x => x.OrderDetails);
            }
            else if (role == nameof(RoleEnum.Customer))
            {
                orders = await _unitOfWork.OrderRepository.WhereAsync(x =>
                       x.StoreId == storeId &&
                       x.ModificatedBy != _claimsService.GetCurrentUser,
                       x => x.Store, x => x.Station, x => x.Payment, x => x.OrderDetails);
            }
            else
            {
                orders = await _unitOfWork.OrderRepository.WhereAsync(x =>
                       x.StoreId == storeId,
                       x => x.Store, x => x.Station, x => x.Payment, x => x.OrderDetails);
            }
            if (orders.Count == 0) throw new BadRequestException("No order for store is found!");
            return orders;
        }

        private async Task<IEnumerable<OrderViewModel>> GetOrderDetail(List<OrderViewModel> orders)
        {
            for (var i = 0; i < orders.Count(); i++)
            {
                var orderDetail = await _unitOfWork.OrderDetailRepository.WhereAsync(x => x.OrderId == orders[i].Id, x => x.ProductInMenu.Product);
                orders[i].OrderDetails = _mapper.Map<IEnumerable<OrderDetailViewModel>>(orderDetail);
            }
            return orders;
        }
    }
}