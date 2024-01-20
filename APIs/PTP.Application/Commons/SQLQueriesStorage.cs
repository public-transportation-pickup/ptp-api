namespace PTP.Application.Commons;
public static class SqlQueriesStorage
{
    /// <summary>
    /// Get RouteStation By RouteId and RouteVarId
    /// </summary>
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
    /// Get All Trips by
    /// <param name="id">RouteId</param>
    /// <param name="routeVarId">RouteVarId</param>
    /// </summary>
    public const string GET_TRIPS_BY_PARENTS_ID = @"
                                                    SELECT t.Id,tt.ApplyDates, t.[Name], t.StartTime, 
                                                    t.EndTime, t.[Status] FROM TimeTables tt JOIN Trip t
                                                    ON t.TimeTableId = tt.Id
                                                    WHERE RouteId = @id
                                                    AND RouteVarId = @routeVarId";

    /// <summary>
    /// Get Trip By 
    /// <param name="id">TripId</param>
    /// </summary>                         
    public const string GET_TRIP_BY_ID = @"SELECT t.Id,tt.ApplyDates, t.[Name], t.StartTime, t.EndTime, t.[Status] 
                                                    FROM TimeTables tt JOIN Trip t
                                                    ON t.TimeTableId = tt.Id
                                                    WHERE t.Id = @id";

    /// <summary>
    /// Get Trips By 
    /// <param name="timeTableId">TimeTableId</param> 
    /// </summary>
    public const string GET_TRIP_BY_TIMETABLEID = @"SELECT t.Id,tt.ApplyDates, t.[Name], t.StartTime, t.EndTime, t.[Status] 
                                                    FROM TimeTables tt JOIN Trip t
                                                    ON t.TimeTableId = tt.Id
                                                    WHERE tt.Id = @timeTableId";

    /// <summary>
    /// Get Trip Schedule by TripId
    /// </summary>
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
    /// <summary>
    /// Get Timetable by routeId and RouteVarId
    /// </summary>
    public const string GET_TIMETABLE_BY_PARID = @"
                                                SELECT tt.Id, TimeTableId, tt.IsCurrent, 
                                                tt.ApplyDates, tt.RouteId, 
                                                tt.RouteVarId FROM [TimeTables] tt 
                                                JOIN [Route] r
                                                ON r.Id = tt.RouteId
                                                JOIN [RouteVars] rv
                                                ON tt.RouteVarId = rv.Id
                                                ORDER BY TimeTableId";
    /// <summary>
    /// Get All Route Vars by RouteId
    /// </summary>
    public const string GET_ROUTE_VARS_BY_ROUTE_ID = @"
                                                SELECT rv.Id, RouteVarName, 
                                                RouteVarShortName, StartStop, EndStop,
                                                OutBound, RunningTime, r.RouteId, rv.Distance
                                                FROM RouteVars rv JOIN [Route] r 
                                                ON rv.RouteId = r.Id
                                                WHERE rv.RouteId = @routeId
                                                ORDER BY rv.RouteVarId";
    /// <summary>
    /// CommandText của GET TIMETABLE BY ID
    /// </summary>
    public const string GET_TIMETABLE_BY_ID = @"SELECT * FROM TimeTables WHERE Id = @id";
}