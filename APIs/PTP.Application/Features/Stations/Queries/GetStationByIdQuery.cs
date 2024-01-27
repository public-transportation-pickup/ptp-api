using Dapper;
using FluentValidation;
using MediatR;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Stations;

namespace PTP.Application.Features.Stations.Queries;
public class GetStationByIdQuery : IRequest<StationViewModel?>
{
    public Guid Id { get; set; } = default!;
    public class QueryValidation : AbstractValidator<GetStationByIdQuery> 
    {
        public QueryValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class QueryHandler : IRequestHandler<GetStationByIdQuery, StationViewModel?>
    {
        private readonly IConnectionConfiguration _connection;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _connection = unitOfWork.DirectionConnection;
        }
        public Task<StationViewModel?> Handle(GetStationByIdQuery request, CancellationToken cancellationToken)
        {
            var query = @"SELECT * FROM Station WHERE Id = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", request.Id);
            using var connection = _connection.GetDbConnection();
            var result = connection.QueryFirstOrDefaultAsync<StationViewModel>(
                sql: query,
                param: parameters,
                transaction: null,
                commandTimeout: 30,
                commandType: System.Data.CommandType.Text
            );
            return result ?? throw new Exception($"Error: {nameof(GetStationByIdQuery)}-no_data_found");

        }
    }
}