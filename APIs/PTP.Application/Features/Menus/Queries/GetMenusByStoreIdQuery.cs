using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Menus.Queries;

public class GetMenusByStoreId:IRequest<PaginatedList<MenuViewModel>>
{
    public Guid StoreId { get; set; } = default!;
    public Dictionary<string, string>? Filter { get; set; } = default!;
    public int PageNumber{get;set;}
    public int PageSize{get;set;}

    public class QueryValidation : AbstractValidator<GetMenusByStoreId>
    {
        public QueryValidation()
        {
            RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }

     public class QueryHandler : IRequestHandler<GetMenusByStoreId, PaginatedList<MenuViewModel>>
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
        public async Task<PaginatedList<MenuViewModel>> Handle(GetMenusByStoreId request, CancellationToken cancellationToken)
        {
            request.Filter!.Remove("pageSize");
            request.Filter!.Remove("pageNumber");
            var cacheResult= await GetCache(request);
            if(cacheResult is not null) return cacheResult;

            var menus = await _unitOfWork.MenuRepository.WhereAsync(x=>x.StoreId==request.StoreId,x=>x.Store);
            if (menus is null) throw new BadRequestException($"Store with ID-{request.StoreId} is not exist any menus!");
            await _cacheService.SetByPrefixAsync<Menu>(CacheKey.MENU, menus);
            var viewModels= _mapper.Map<IEnumerable<MenuViewModel>>(menus);
            var filterResult = request.Filter.Count > 0 ? new List<MenuViewModel>() : viewModels;

            if(request.Filter!.Count>0)
            {
                foreach(var filter in request.Filter) 
                {
                    filterResult=filterResult.Union(FilterUtilities.SelectItems(viewModels, filter.Key, filter.Value));
                }
            }
            
            return PaginatedList<MenuViewModel>.Create(
                        source:filterResult.AsQueryable(),
                        pageIndex:request.PageNumber,
                        pageSize:request.PageSize
                );
        }

        public async Task<PaginatedList<MenuViewModel>?> GetCache(GetMenusByStoreId request)
            {
                
                if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");

                var cacheResult = await _cacheService.GetByPrefixAsync<Menu>(CacheKey.MENU);
                if (cacheResult!.Count > 0)
                {
                    var result = cacheResult.Where(x => x.StoreId == request.StoreId);
                    if(result == null) return null;
                    var cacheViewModels= _mapper.Map<IEnumerable<MenuViewModel>>(result);   
                    var filterRe= request.Filter!.Count > 0 ? new List<MenuViewModel>(): cacheViewModels;
                    if(request.Filter!.Count>0)
                    {
                        foreach(var filter in request.Filter) 
                        {
                            filterRe=filterRe.Union(FilterUtilities.SelectItems(cacheViewModels, filter.Key, filter.Value));
                        }
                    }               
                    return PaginatedList<MenuViewModel>.Create(
                            source: filterRe.AsQueryable(),
                            pageIndex:request.PageNumber,
                            pageSize:request.PageSize
                    );
                }
                return null;
            }
    }
}