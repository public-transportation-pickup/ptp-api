namespace PTP.Application.Commons;
public static class SqlQueriesStorage
{
    public const string GET_ROUTE_STATION_BY_PARENT_ID = @"
                                        SELECT rs.Id, rs.[Index], s.Latitude, s.Longitude, 
                                        rs.DistanceFromStart, rs.DistanceToNext, rs.DurationFromStart, 
                                        rs.DurationToNext, s.[Id] AS StationId, s.[Name] AS StationName
                                        FROM RouteStation rs
                                        JOIN Station s
                                        ON s.Id = rs.StationId 
                                        WHERE RouteId = @id
                                        AND RouteVarId = @routeVarId
                                        ORDER BY [Index]";

    /// <summary>
    /// Get All Trip by RouteId and RouteVarId
    /// <param name="id">routeId</param>
    /// <param name="routeVarId">RouteVarId</param>
    /// </summary>
    public const string GET_TRIPS_BY_PARENTS_ID = @"
                                                    SELECT t.Id,tt.ApplyDates, t.[Name], t.StartTime, t.EndTime, t.[Status] FROM TimeTables tt JOIN Trip t
                                                    ON t.TimeTableId = tt.Id
                                                    WHERE RouteId = @id
                                                    AND RouteVarId = @routeVarId";

    /// <summary>
    /// Get Trip By Id
    /// <param name="id">Trip Id</param>
    /// </summary>                         
    public const string GET_TRIP_BY_ID = @"SELECT t.Id,tt.ApplyDates, t.[Name], t.StartTime, t.EndTime, t.[Status] 
                                                    FROM TimeTables tt JOIN Trip t
                                                    ON t.TimeTableId = tt.Id
                                                    WHERE t.Id = @id";

    public const string GET_TRIP_SCHEDULE_BY_ID = @"SELECT rs.[Index] ,rs.DistanceFromStart, rs.DistanceToNext, rs.DurationFromStart, 
                                                    rs.DurationToNext, s.[Name] AS StationName,
                                                    DATEADD(MINUTE, rs.DurationFromStart, PARSE(t.StartTime AS time)) AS ArrivalTime
                                                    FROM RouteStation rs
                                                    JOIN Station s
                                                    ON s.Id = rs.StationId 
                                                    JOIN RouteVars rv ON rs.RouteVarId = rv.Id
                                                    JOIN TimeTables tt ON tt.RouteVarId = rv.Id
                                                    JOIN Trip t ON tt.Id = t.TimeTableId
                                                    WHERE t.Id = @id
                                                    ORDER BY [Index]";
}