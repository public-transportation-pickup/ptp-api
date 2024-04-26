using AutoMapper;
using FluentValidation;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.ProductMenus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.ProductMenus.Queries;

public class GetProductInMenuByMenuIdQuery : IRequest<Pagination<ProductMenuViewModel>>
{
    public Guid MenuId { get; set; }
    //public Guid CategoryId{get;set;}=default!;
    public Dictionary<string, string>? Filter { get; set; } = default!;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public class CommmandValidation : AbstractValidator<GetProductInMenuByMenuIdQuery>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.MenuId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }

    }

    public class CommandHandler : IRequestHandler<GetProductInMenuByMenuIdQuery, Pagination<ProductMenuViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        public CommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<Pagination<ProductMenuViewModel>> Handle(GetProductInMenuByMenuIdQuery request, CancellationToken cancellationToken)
        {
            request.Filter!.Remove("pageSize");
            request.Filter!.Remove("pageNumber");

            var cacheResult = await GetCache(request);

            var productMenus = cacheResult != null ? cacheResult : await _unitOfWork.ProductInMenuRepository
                                .WhereAsync(x => x.MenuId == request.MenuId,
                                            x => x.Menu,
                                            x => x.Product,
                                            x => x.Product.Category);

            if (productMenus.Count == 0) throw new NotFoundException($"There are no product for Menu-{request.MenuId}!");
            await _cacheService.SetByPrefixAsync<ProductInMenu>(CacheKey.PRODUCTMENU, productMenus);
            var viewModels = _mapper.Map<IEnumerable<ProductMenuViewModel>>(productMenus);
            var filterResult = request.Filter.Count > 0 ? new List<ProductMenuViewModel>() : viewModels;

            if (request.Filter!.Count > 0)
            {
                foreach (var filter in request.Filter)
                {
                    filterResult = filterResult.Union(FilterUtilities.SelectItems(viewModels, filter.Key, filter.Value));
                }
            }
            return new Pagination<ProductMenuViewModel>
            {
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                TotalItemsCount = filterResult.Count(),
                Items = PaginatedList<ProductMenuViewModel>.Create(
                        source: filterResult.AsQueryable(),
                        pageIndex: request.PageNumber,
                        pageSize: request.PageSize
                )
            };

        }

        public async Task<List<ProductInMenu>?> GetCache(GetProductInMenuByMenuIdQuery request)
        {

            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");

            var cacheResult = await _cacheService.GetByPrefixAsync<ProductInMenu>(CacheKey.PRODUCTMENU);
            if (cacheResult!.Count > 0)
            {
                var result = cacheResult.Where(x => x.MenuId == request.MenuId).ToList();
                return !result.Any() ? null : result;

            }
            return null;
        }
    }
}