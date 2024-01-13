using AutoMapper;
using Dapper;
using MediatR;
using PTP.Application.Data.Configuration;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.ViewModels.RouteVars;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Routes.Queries;
public class GetRouteVarByRouteIdQuery : IRequest<IEnumerable<RouteVarViewModel>>
{
	public Guid Id { get; set; } = default!;
	public class QueryHandler : IRequestHandler<GetRouteVarByRouteIdQuery, IEnumerable<RouteVarViewModel>>
	{
		private readonly IConnectionConfiguration _connConf;
		private readonly IMapper _mapper;
		public QueryHandler(IConnectionConfiguration connConf, IMapper mapper)
		{
			_connConf = connConf;
			_mapper = mapper;
		}
		public async Task<IEnumerable<RouteVarViewModel>> Handle(GetRouteVarByRouteIdQuery request, CancellationToken cancellationToken)
		{
			using var conn = _connConf.GetDbConnection();
			var parameters = new DynamicParameters();
			var query = @"SELECT * FROM [RouteVars] WHERE RouteId = @routeId";
			parameters.Add("@routeId", request.Id);
			var result = await conn.QueryAsync<RouteVar>(query, parameters, commandTimeout: 90, commandType: System.Data.CommandType.Text);
			if (result?.Count() > 0)
			{
				return _mapper.Map<IEnumerable<RouteVarViewModel>>(result);
			}
			else
			{
				throw new NotFoundException("no_data_found");
			}
		}
	}
}