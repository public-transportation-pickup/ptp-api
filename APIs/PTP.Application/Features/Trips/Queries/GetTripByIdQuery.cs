using Dapper;
using MediatR;
using PTP.Application.Commons;
using PTP.Application.Data.Configuration;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Utilities;
using PTP.Application.ViewModels.Schedules;
using PTP.Application.ViewModels.Trips;

namespace PTP.Application.Features.Trips.Queries;
public class GetTripByIdQuery : IRequest<TripViewModel>
{
    public Guid Id { get; set; } = default!;
    public bool IsSchedule { get; set; }
    public class QueryHandler : IRequestHandler<GetTripByIdQuery, TripViewModel>
    {
        private readonly IConnectionConfiguration connectionConfiguration;
        public QueryHandler(IConnectionConfiguration connectionConfiguration)
        {
            this.connectionConfiguration = connectionConfiguration;
        }
        public async Task<TripViewModel> Handle(GetTripByIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = connectionConfiguration.GetDbConnection();
            var query = SqlQueriesStorage.GET_TRIP_BY_ID;
            var parameters = new DynamicParameters();
            parameters.Add("@id", request.Id);

            var result = await connection.QueryFirstOrDefaultAsync<TripViewModel>(query, parameters) ??
                            throw new NotFoundException($"no_data_found at {nameof(GetTripByIdQuery)}");
            if (request.IsSchedule)
            {
                if (!string.IsNullOrEmpty(result.ApplyDates))
                {
                    if (!result.ApplyDates.CheckDayActive())
                    {
                        throw new Exception($"Trip {result.Id} does not active on this date {DateTime.Now}, ApplyDates: {result.ApplyDates}");
                    }
                }
                else throw new Exception($"Trip {result.Id} not have ApplyDates");
            }

            result.Schedules = request.IsSchedule ? await GetScheduleAsync(request.Id) : new List<ScheduleViewModel>();
            return result;

        }
        private async Task<IEnumerable<ScheduleViewModel>> GetScheduleAsync(Guid tripId)
        {
            using var connection = connectionConfiguration.GetDbConnection();
            var query = SqlQueriesStorage.GET_TRIP_SCHEDULE_BY_ID;
            var parameters = new DynamicParameters();
            parameters.Add("@id", tripId);
            var result = await connection.QueryAsync<ScheduleViewModel>(query, parameters);
            if (result?.Count() > 0)
            {
                var resultArr = result.ToArray();

                // TODO: Cập nhật lại time, time và distance nếu đo từ start là không chuẩn
                for (int i = 0; i < resultArr.Length; i++)
                {
                    if (i == 0) continue;
                    resultArr[i].ArrivalTime = resultArr[i -1].ArrivalTime.Add(TimeSpan.FromMinutes((double)resultArr[i - 1].DurationToNext));
                }
                return resultArr.ToList();
            }
            else throw new Exception($"no_data_found at {nameof(GetTripByIdQuery)}_{nameof(GetScheduleAsync)}");

        }
    }
}