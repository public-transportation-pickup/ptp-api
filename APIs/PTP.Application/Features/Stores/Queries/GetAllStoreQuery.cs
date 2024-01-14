using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Stores;
using PTP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.Features.Stores.Queries
{
    public class GetAllStoreQuery : IRequest<IEnumerable<StoreViewModel>>
    {
        public class QueryHandler : IRequestHandler<GetAllStoreQuery, IEnumerable<StoreViewModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ICacheService  _cacheService;
            private ILogger<QueryHandler> logger;
            private const string CACHE_KEY = "STORE";

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
                var cacheResult = await _cacheService.GetByPrefixAsync<Store>(CACHE_KEY);
                if (cacheResult!.Count > 0)
                {
                    return _mapper.Map<IEnumerable<StoreViewModel>>(cacheResult);
                }
                var stores = await _unitOfWork.StoreRepository.GetAllAsync();
                if (stores.Count == 0) throw new NotFoundException("There are no store in DB!");
                await _cacheService.SetByPrefixAsync<Store>(CACHE_KEY, stores);
                return _mapper.Map<IEnumerable<StoreViewModel>>(stores);
            }
        }
    }
}
