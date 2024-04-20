using System.Drawing;
using AutoMapper;
using Dapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Routes;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Routes.Queries;
public class GetRouteByIdQuery : IRequest<RouteViewModel>
{
    public Guid Id { get; set; } = default!;
    public class QueryValidation : AbstractValidator<GetRouteByIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }
    public class QueryHandler : IRequestHandler<GetRouteByIdQuery, RouteViewModel>
    {
        private readonly IConnectionConfiguration _connectionConfig;
        private readonly IMapper _mapper;
        public QueryHandler(IConnectionConfiguration connectionConfiguration, IMapper mapper)
        {
            _mapper = mapper;
            _connectionConfig = connectionConfiguration;
        }
        public async Task<RouteViewModel> Handle(GetRouteByIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connectionConfig.GetDbConnection();
            var query = "SELECT * FROM [Route] WHERE Id = @Id AND IsDeleted = 0";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", request.Id);
            var route = await connection.QueryAsync<Route>(query, parameters, null, 90, System.Data.CommandType.Text) ?? throw new Exception("no_data_found");
            if (route.Count() > 1)
            {
                throw new Exception("more_than_one_result");
            }
            else return _mapper.Map<RouteViewModel>(route.First());

        }
    }
}