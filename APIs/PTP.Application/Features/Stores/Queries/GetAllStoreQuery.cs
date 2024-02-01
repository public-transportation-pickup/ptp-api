using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Stores;
using PTP.Domain.Entities;
using PTP.Domain.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.Features.Stores.Queries
{
    public class GetAllStoreQuery : IRequest<IEnumerable<StoreViewModel>>
    {
        public Dictionary<string, string>? Filter { get; set; } = default!;
        public class QueryHandler : IRequestHandler<GetAllStoreQuery, IEnumerable<StoreViewModel>>
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

            public async Task<IEnumerable<StoreViewModel>> Handle(GetAllStoreQuery request, CancellationToken cancellationToken)
            {
                if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
                var cacheResult = await _cacheService.GetByPrefixAsync<Store>(CacheKey.STORE);
                if (cacheResult!.Count > 0)
                {
                    return _mapper.Map<IEnumerable<StoreViewModel>>(cacheResult);
                }
                var stores = await _unitOfWork.StoreRepository.GetAllAsync(x=>x.User,x=>x.Wallet!);
                if (stores.Count == 0) throw new NotFoundException("There are no store in DB!");
                await _cacheService.SetByPrefixAsync<Store>(CacheKey.STORE, stores);
                var viewModels= _mapper.Map<IEnumerable<StoreViewModel>>(stores);
                if(request.Filter!.Count>0)
                {
                    foreach(var filter in request.Filter) 
                    {
                        return viewModels.Union(FilterUtilities.SelectItems(viewModels, filter.Key, filter.Value));
                    }
                }
                
                return viewModels;
            }
        }
    }
}
