using System.Security.Principal;
using System.Data;
using MediatR;
using Microsoft.AspNetCore.Components;
using PTP.Application.ViewModels.Routes;
using AutoMapper;
using PTP.Application.Data.Configuration;
using Dapper;
using PTP.Domain.Entities;
using PTP.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace PTP.Application.Features.Routes.Queries;
public class GetAllRouteQuery : IRequest<IEnumerable<RouteViewModel>>
{

    public class QueryHandler : IRequestHandler<GetAllRouteQuery, IEnumerable<RouteViewModel>>
    {
        private readonly IConnectionConfiguration _connection;
        private readonly IMapper _mapper;
        private ICacheService _cacheService;
        private const string CACHE_KEY = "ALL_ROUTE";
        private ILogger<QueryHandler> logger;
        public QueryHandler(IMapper mapper, IConnectionConfiguration connection, ICacheService cacheService, ILogger<QueryHandler> logger)
        {
            _mapper = mapper;
            this.logger = logger;
            _cacheService = cacheService;
            _connection = connection;
        }
        public async Task<IEnumerable<RouteViewModel>> Handle(GetAllRouteQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connection.GetDbConnection();
            if (_cacheService.IsConnected())
            {
                logger.LogInformation("Have Cache");
                var cacheResult = await _cacheService.GetAsync<IEnumerable<Route>>(CACHE_KEY);
                if (cacheResult is null)
                {
                    string query = @"SELECT * FROM [Route] ORDER BY RouteId";
                    var result = await connection.QueryAsync<Route>(query);
                    if (result is not null && result.Count() > 0)
                    {
                        if (_cacheService.IsConnected())
                        {
                            await _cacheService.SetAsync(CACHE_KEY, result);
                        }
                        return _mapper.Map<IEnumerable<RouteViewModel>>(result);
                    }
                    else throw new Exception("Result is null");
                }
                else return _mapper.Map<IEnumerable<RouteViewModel>>(cacheResult);
            }
            else
            {
                string query = @"SELECT * FROM [Route] ORDER BY RouteId";
                var result = await connection.QueryAsync<Route>(query);
                if (result is not null && result.Count() > 0)
                {

                    return _mapper.Map<IEnumerable<RouteViewModel>>(result);
                }
                else throw new Exception("Result is null");
            }

        }
    }
}