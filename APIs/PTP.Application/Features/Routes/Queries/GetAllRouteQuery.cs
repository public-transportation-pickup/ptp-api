using System.Text.Json.Serialization;
using AutoMapper;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.Services.Interfaces;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Routes;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Routes.Queries;
public class GetAllRouteQueryModel
{
	public Dictionary<string, string> Filter { get; set; } = new();

	public int PageNumber { get; set; } = -1;
}
public class GetAllRouteQuery : GetAllRouteQueryModel, IRequest<PaginatedList<RouteViewModel>>
{

	public class QueryHandler : IRequestHandler<GetAllRouteQuery, PaginatedList<RouteViewModel>>
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
		public async Task<PaginatedList<RouteViewModel>> Handle(GetAllRouteQuery request, CancellationToken cancellationToken)
		{

			using var connection = _connection.GetDbConnection();
			IEnumerable<RouteViewModel> result;
			if (!_cacheService.IsConnected())
			{
				logger.LogInformation("Have Cache");
				var cacheResult = await _cacheService.GetAsync<IEnumerable<Route>>(CACHE_KEY);
				if (cacheResult is null)
				{
					string query = @"SELECT * FROM [Route] WHERE IsDeleted = 0  ORDER BY RouteId ";
					var resultInDb = await connection.QueryAsync<Route>(query);
					if (resultInDb is not null && resultInDb.Count() > 0)
					{
						result = _mapper.Map<IEnumerable<RouteViewModel>>(resultInDb);
						if (!_cacheService.IsConnected())
						{
							await _cacheService.SetAsync(CACHE_KEY, resultInDb);
						}

					}
					else throw new Exception("Result is null");
				}
				else result = _mapper.Map<IEnumerable<RouteViewModel>>(cacheResult);
			}
			else
			{

				string query = @"SELECT * FROM [Route] WHERE IsDeleted = 0 ORDER BY CreationDate DESC";
				var resultInDb = await connection.QueryAsync<Route>(query);
				if (resultInDb is not null && resultInDb.Any())
				{
					result = _mapper.Map<IEnumerable<RouteViewModel>>(resultInDb);
				}
				else throw new Exception("Result is null");
			}
			List<RouteViewModel> returnResult = new();
			if(request.Filter.TryGetValue("pageNumber", out var value))
			request.Filter.Remove("pageNumber");
			if (request.Filter?.Count > 0)
			{
				
				
				foreach (var item in request.Filter)
				{
					System.Console.WriteLine(item.Key);
					// TODO, Loop to find matching 
					//System.Console.WriteLine(FilterUtilities.SelectItems(result, item.Key, item.Value).ToList().Count);

					returnResult = returnResult.Union(FilterUtilities.SelectItems(result, item.Key, item.Value).ToList()).ToList();

				}
				
			}
			else returnResult = result.ToList();
			logger.LogInformation($"Result: {result?.Count()}");
			return PaginatedList<RouteViewModel>.Create(
							source: returnResult.AsQueryable(),
							pageIndex: request.PageNumber == -1 ? 0 : request.PageNumber,
							pageSize: request.PageNumber == -1 ? returnResult.Count : 10);

		}
	}
}