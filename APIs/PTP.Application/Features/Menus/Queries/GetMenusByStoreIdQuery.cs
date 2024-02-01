using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Menus.Queries;

public class GetMenusByStoreId:IRequest<IEnumerable<MenuViewModel>>
{
    public Guid StoreId { get; set; } = default!;

    public class QueryValidation : AbstractValidator<GetMenusByStoreId>
    {
        public QueryValidation()
        {
            RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }

     public class QueryHandler : IRequestHandler<GetMenusByStoreId, IEnumerable<MenuViewModel>>
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
        public async Task<IEnumerable<MenuViewModel>> Handle(GetMenusByStoreId request, CancellationToken cancellationToken)
        {
            if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            var cacheResult = await _cacheService.GetByPrefixAsync<Menu>(CacheKey.MENU);
            if (cacheResult!.Count > 0)
            {
                //var result = cacheResult.Select(x => x.StoreId == request.StoreId);
                var result = cacheResult.Where(x => x.StoreId == request.StoreId);
                return _mapper.Map<IEnumerable<MenuViewModel>>(result);
            }
            var menus = await _unitOfWork.MenuRepository.WhereAsync(x=>x.StoreId==request.StoreId,x=>x.Store);
            if (menus is null) throw new BadRequestException($"Store with ID-{request.StoreId} is not exist any menus!");
            await _cacheService.SetByPrefixAsync<Menu>(CacheKey.MENU, menus);
            return (_mapper.Map<IEnumerable<MenuViewModel>>(menus)).OrderBy(x=>x.StartTime);
        }
    }
}