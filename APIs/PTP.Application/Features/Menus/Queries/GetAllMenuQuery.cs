using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Menus.Queries;

public class GetAllMenuQuery:IRequest<IEnumerable<MenuViewModel>>
{
    public class QueryHandler : IRequestHandler<GetAllMenuQuery, IEnumerable<MenuViewModel>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService  _cacheService;
        private ILogger<QueryHandler> logger;

        public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<QueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            this.logger = logger;
        }

        public async Task<IEnumerable<MenuViewModel>> Handle(GetAllMenuQuery request, CancellationToken cancellationToken)
        {
            if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            var cacheResult = await _cacheService.GetByPrefixAsync<Menu>(CacheKey.MENU);
            if (cacheResult!.Count > 0)
            {
                return _mapper.Map<IEnumerable<MenuViewModel>>(cacheResult);
            }
            var menus = await _unitOfWork.MenuRepository.GetAllAsync(x=>x.Store);
            if (menus.Count == 0) throw new NotFoundException("There are no menu in DB!");
            await _cacheService.SetByPrefixAsync<Menu>(CacheKey.MENU, menus);
            return _mapper.Map<IEnumerable<MenuViewModel>>(menus);
        }
    }
}

