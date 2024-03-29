using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Features.ProductMenus.Queries;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Menus;
using PTP.Application.ViewModels.ProductMenus;
using PTP.Domain.Entities;
using System.Globalization;

namespace PTP.Application.Features.Menus.Queries
{
    public class GetMenuDetailByStoreId : IRequest<ProductsStore>
    {
        public Guid StoreId { get; set; } = default!;
        public string ArrivalTime { get; set; } = default!;

        public DateTime DateApply { get; set; } = default!;
        public class QueryValidation : AbstractValidator<GetMenuDetailByStoreId>
        {
            public QueryValidation()
            {
                RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetMenuDetailByStoreId, ProductsStore>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ICacheService _cacheService;
            private readonly IMediator _mediator;
            private ILogger<QueryHandler> _logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, IMediator mediator, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _cacheService = cacheService;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<ProductsStore> Handle(GetMenuDetailByStoreId request, CancellationToken cancellationToken)
            {
                var menus = await _unitOfWork.MenuRepository.WhereAsync(x =>
                                                 x.StoreId == request.StoreId);
                var store = await _unitOfWork.StoreRepository.GetByIdAsync(request.StoreId);

                if (menus.Count == 0) return new();
                var menuIds = CheckMenu(menus, request);
                var result = new ProductsStore
                {
                    StoreId = store!.Id,
                    Name = store!.Name,
                    Description = store!.Description,
                    PhoneNumber = store!.PhoneNumber,
                    Status = store!.Status
                };
                result.ProductInMenus = await GetProductsInMenu(menuIds);
                result.Categories = result.ProductInMenus?.ToList().ConvertAll<object>(x =>
                {

                    return new
                    {
                        x.CategoryId,
                        x.CategoryName
                    };
                }).Distinct().ToList();
                return result;
            }

            private async Task<List<ProductMenuViewModel>?> GetProductsInMenu(List<Guid> menuIds)
            {
                var productMenus = await _unitOfWork.ProductInMenuRepository
                                .WhereAsync(x => menuIds.Contains(x.MenuId),
                                            x => x.Menu,
                                            x => x.Product,
                                            x => x.Product.Category);

                if (productMenus.Count == 0) return new();
                return _mapper.Map<List<ProductMenuViewModel>>(productMenus);

            }

            private List<Guid> CheckMenu(IEnumerable<Menu> menus, GetMenuDetailByStoreId request)
            {
                var menuIds = new List<Guid>();
                TimeSpan.TryParseExact(request.ArrivalTime, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan aTime);

                foreach (var item in menus)
                {
                    if (item.IsApplyForAll)
                    {
                        if (item.StartTime < aTime && item.EndTime > aTime)
                        {
                            menuIds.Add(item.Id);
                        }
                    }
                    else if (item.StartDate == null && item.EndDate == null)
                    {
                        if (item.StartTime < aTime && item.EndTime > aTime && item.DateApply.Contains(request.DateApply.DayOfWeek.ToString()))
                        {
                            menuIds.Add(item.Id);
                        }
                    }
                    else
                    {
                        if (item.StartDate < request.DateApply && item.EndDate > request.DateApply)
                        {
                            if (item.StartTime < aTime && item.EndTime > aTime && item.DateApply.Contains(request.DateApply.DayOfWeek.ToString()))
                            {
                                menuIds.Add(item.Id);
                            }
                        }
                    }
                }
                return menuIds;
            }

        }
    }
}
