using System.Security.Principal;
using System.Data;
using MediatR;
using Microsoft.AspNetCore.Components;
using PTP.Application.ViewModels.Routes;
using AutoMapper;
using PTP.Application.Data.Configuration;
using Dapper;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Routes.Queries;
public class GetAllRouteQuery : IRequest<IEnumerable<RouteViewModel>>
{

    public class QueryHandler : IRequestHandler<GetAllRouteQuery, IEnumerable<RouteViewModel>>
    {
        private readonly IConnectionConfiguration _connection;
        private readonly IMapper _mapper;
        public QueryHandler(IMapper mapper, IConnectionConfiguration connection)
        {
            _mapper = mapper;
            _connection = connection;
        }
        public async Task<IEnumerable<RouteViewModel>> Handle(GetAllRouteQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connection.GetDbConnection();
            string query = @"SELECT * FROM [Route]";
            var result = await connection.QueryAsync<Route>(query);
            if(result is not null && result.Count() > 0)
            {
                return _mapper.Map<IEnumerable<RouteViewModel>>(result);
            } else throw new Exception("Result is null");
        }
    }
}