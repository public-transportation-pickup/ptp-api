using System.Reflection.Metadata;

namespace PTP.Application.Commons;
public static class SqlQueriesStorage
{
    public const string GET_STATIONS_REVENUE = @"SELECT *
        FROM Station s 
        LEFT JOIN
        (
            SELECT  s.Id AS [StationId], 
                COUNT(CASE WHEN o.[Status] = 'Completed' THEN o.Id END) AS OrderCompleted,
                COUNT(CASE WHEN o.[Status] = 'Canceled' THEN o.Id END) AS OrderCanceled,
                COUNT(CASE WHEN o.[Status] != 'Canceled' AND o.Status != 'Completed' THEN o.Id END) AS OrderOthers
            FROM Station s INNER JOIN [Order] o ON s.Id = o.StationId
            WHERE o.[Status] IS NOT NULL
            GROUP BY s.Id 
            ) oc
        ON oc.StationId = s.Id
        LEFT JOIN 
        (
            SELECT s.Id, SUM(CASE WHEN o.[Status] = 'Completed' THEN o.Total END) AS Revenue
                            FROM [Order] o INNER JOIN [Station] s
                            ON o.StationId = s.Id
                            GROUP BY s.Id
        ) osr 
        ON osr.Id = s.Id
        INNER JOIN
        (
            SELECT [Id], [Name] AS StoreName
            FROM Store
        ) store 
        ON store.Id = s.StoreId
        ORDER BY oc.OrderCompleted DESC";
    public const string GET_TOP_PRODUCT_BY_USER = @"SELECT TOP 5 pm.Id AS ProductMenuId , p.StoreId AS StoreId,p.ImageURL AS ImageURL, p.Name, od.ActualPrice, COUNT(od.Id) AS OrderCount
            FROM [OrderDetails] od INNER JOIN 
            ProductInMenu pm ON od.ProductMenuId = pm.Id
            INNER JOIN Product p 
            ON pm.ProductId = p.Id
            WHERE od.OrderId IN 
            (
                SELECT Id 
                FROM [Order] o 
                WHERE [Status] = 'Completed'
                AND UserID = @UserId
            )
            GROUP BY pm.Id, p.Name, od.ActualPrice, p.ImageURL, p.StoreId
            ORDER BY OrderCount DESC";

    public const string GET_CATEGORIES_REPORT = @"SELECT TOP 5 c.Name, c.ImageURL, COUNT(p.Id) AS Products
        FROM Category c LEFT JOIN 
        Product p
        ON c.Id = p.CategoryId
        Where c.ISDELETED = 0
        GROUP BY c.ImageURL, c.Name
        ORDER BY Products DESC";
    /// <summary>
    /// Lấy tổng Station, Routes
    /// </summary>

    public const string GET_SUM_ROUTES = @"SELECT * FROM
        (SELECT COUNT(*) Stations FROM Station) a,
        (SELECT COUNT(*) Routes FROM [Route]) b,
        (SELECT COUNT(*) Stores FROM [Store]) c, 
        (
            SELECT SUM
            (
                CASE WHEN o.[Status] = 'Completed' THEN o.Total 
                    ELSE 
                        CASE WHEN o.[Status] = 'Canceled' 
                        THEN 
                            CASE WHEN o.ReturnAmount IS NULL
                                THEN o.Total
                            ELSE (o.Total - o.ReturnAmount)
                            END
                        END
                    END
            ) Revenue
            FROM [Order] o 
        ) d";
    /// <summary>
    /// Thống kê top 20 station có nhiều order nhất
    /// </summary>
    public const string GET_TOP_ORDER_STATION = @"SELECT TOP 20 s.Name AS [Name], 
        COUNT(CASE WHEN o.[Status] = 'Completed' THEN o.Id END) AS OrderCompleted,
        COUNT(CASE WHEN o.[Status] = 'Canceled' THEN o.Id END) AS OrderCanceled,
        COUNT(CASE WHEN o.[Status] != 'Canceled' AND o.Status != 'Completed' THEN o.Id END) AS OrderOthers
        FROM Station s INNER JOIN [Order] o
        ON s.Id = o.StationId
        WHERE o.[Status] IS NOT NULL
        GROUP BY s.Name
        ORDER BY OrderCompleted DESC, OrderCanceled DESC";
    /// <summary>
    /// Lấy thống kê top 20 cửa hàng có nhiều ORDER nhất
    /// </summary>
    public const string GET_TOP_ORDER_STORES = @"SELECT TOP 5 a.Name, a.Address, a.OrderCompleted, a.OrderCanceled, a.OrderOthers, b.Revenue
            FROM 
            (SELECT s.Name, s.Address,
                COUNT(CASE WHEN o.[Status] = 'Completed' THEN o.Id END) AS OrderCompleted,
                COUNT(CASE WHEN o.[Status] = 'Canceled' THEN o.Id END) AS OrderCanceled,
                COUNT(CASE WHEN o.[Status] != 'Canceled' AND o.Status != 'Completed' THEN o.Id END) AS OrderOthers
            FROM
                (SELECT s.Id, s.Name AS [Name], s.AddressNo + ', ' + s.Street + ', ' + s.Ward + ', ' + s.[Zone]  AS [Address]
                FROM Store s) s INNER JOIN [Order] o
                ON s.Id = o.StoreId
            WHERE o.[Status] IS NOT NULL
            GROUP BY s.Name, s.Address) a
            INNER JOIN (SELECT TOP 5 s.Name, SUM(
                 CASE WHEN o.[Status] = 'Completed' THEN o.Total 
                    ELSE 
                        CASE WHEN o.[Status] = 'Canceled' 
                        THEN 
                            CASE WHEN o.ReturnAmount IS NULL
                                THEN o.Total
                            ELSE (o.Total - o.ReturnAmount)
                            END
                        END
                    END
                ) AS Revenue
                    FROM [Order] o INNER JOIN [Store] s
                    ON o.StoreId = s.Id
                    GROUP BY s.Name) b 
            ON a.Name = b.Name
            ORDER BY Revenue DESC";

    /// <summary>
    /// Lấy thống kê top5 cửa hàng có doanh thu cao nhất
    /// </summary>
    public const string GET_TOP_PROFIT_STORES = @"
            SELECT TOP 5 s.Name, 
            SUM
                (
                    CASE WHEN o.[Status] = 'Completed' THEN o.Total 
                    ELSE 
                        CASE WHEN o.[Status] = 'Canceled' 
                        THEN 
                            CASE WHEN o.ReturnAmount IS NULL
                                THEN o.Total
                            ELSE (o.Total - o.ReturnAmount)
                            END
                        END
                    END
                ) AS Revenue
            FROM [Order] o INNER JOIN [Store] s
            ON o.StoreId = s.Id
            WHERE o.CreationDate >= DATEADD(day, -(DATEPART(dw, GETDATE()) + 5) % 7, CAST(GETDATE() AS date))
                AND o.CreationDate < DATEADD(day, 7 - (DATEPART(dw, GETDATE()) + 5) % 7, CAST(GETDATE() AS date))
                    GROUP BY s.Name
                    ORDER BY Revenue DESC";
    public const string GET_ROUTE_BY_STATION_NAME = @"
        SELECT DISTINCT (r.Id), r.RouteId, r.Name,r.[Status], 
            r.RouteNo, r.Distance, r.TimeOfTrip, r.HeadWay, r.OperationTime,
            r.NumOfSeats, r.InBoundName, r.OutBoundName, r.TotalTrip, r.Orgs, r.Tickets 
        FROM [Route] r 
        LEFT JOIN RouteStation rs ON r.Id = rs.RouteId
        INNER JOIN Station s ON s.Id = rs.StationId
        WHERE s.Name LIKE @stationName
        ORDER BY RouteNo";
    
    public const string GET_ROUTE_BY_ADDRESS = @"
        SELECT DISTINCT (r.Id), r.RouteId, r.Name,r.[Status], 
            r.RouteNo, r.Distance, r.TimeOfTrip, r.HeadWay, r.OperationTime,
            r.NumOfSeats, r.InBoundName, r.OutBoundName, r.TotalTrip, r.Orgs, r.Tickets 
        FROM [Route] r 
        LEFT JOIN RouteStation rs ON r.Id = rs.RouteId
        INNER JOIN Station s ON s.Id = rs.StationId
        WHERE s.AddressNo + ' ' + s.Street + ' ' + s.Ward + ' ' + s.Zone
         LIKE @stationName
        ORDER BY RouteNo"; 
    public const string GET_STORES_BY_ROUTEVARID = @"
        SELECT *
        FROM [Store] str 
        WHERE str.Id IN 
            (
                SELECT s.StoreId
                    FROM RouteStation rs
                    INNER JOIN Station s
                    ON rs.StationId = s.Id
                    WHERE rs.RouteVarId = @routeVarId
                    AND s.ISDELETED = 0
            )";
    public const string GET_STATION_BY_ROUTEVARID = @"
                                            SELECT s.Id, s.[Name], rs.[Index], s.[Latitude], s.[Longitude],
                                            s.Address, s.Code
                                            FROM RouteStation rs
                                            INNER JOIN Station s
                                            ON rs.StationId = s.Id
                                            WHERE rs.RouteVarId = @routeVarId
                                            AND s.ISDELETED = 0
                                            ORDER BY rs.[Index]";
    public const string GET_ALL_USER = @"
                                        SELECT u.Id, FullName, 
                                        [Email], PhoneNumber, 
                                        DateOfBirth, r.[Name] AS RoleName
                                        FROM [User] u LEFT JOIN
                                        Role r
                                        ON u.RoleID = r.Id
                                        WHERE u.IsDeleted = 0";
    public const string GET_USER_BY_ID = @"
                                        SELECT u.Id, FullName, 
                                        [Email], PhoneNumber, 
                                        DateOfBirth, r.[Name] AS RoleName, 
                                        u.StoreId AS StoreId, s.[Name] AS StoreName
                                        FROM [User] u LEFT JOIN
                                        Role r
                                        ON u.RoleID = r.Id
                                        LEFT JOIN Store s 
                                        ON s.Id = u.StoreId
                                        WHERE u.Id = @id
                                        AND u.IsDeleted = 0";
    /// <summary>
    /// Get RouteStation By RouteId and RouteVarId
    /// </summary>
    public const string GET_ROUTE_STATION_BY_PARENT_ID = @"
                                        SELECT rs.Id, rs.[Index], s.Latitude, s.Longitude, 
                                        rs.DistanceFromStart, rs.DistanceToNext, rs.DurationFromStart, 
                                        rs.DurationToNext, s.[Id] AS StationId, s.[Name] AS StationName, s.StoreId
                                        FROM RouteStation rs
                                        JOIN Station s
                                        ON s.Id = rs.StationId 
                                        WHERE RouteId = @id
                                        AND rs.IsDeleted = 0
                                        AND RouteVarId = @routeVarId
                                        AND rs.IsDeleted = 0
                                        ORDER BY [Index]";
    /// <summary>
    /// Get RouteStation By RouteVarId
    /// </summary>
    public const string GET_ROUTE_STATION_BY_ROUTEVAR_ID = @"SELECT rs.Id, rs.[Index], s.Latitude, s.Longitude, 
                                        rs.DistanceFromStart, rs.DistanceToNext, rs.DurationFromStart, 
                                        rs.DurationToNext, s.[Id] AS StationId, s.[Name] AS StationName
                                        FROM RouteStation rs
                                        JOIN Station s
                                        ON s.Id = rs.StationId 
                                        WHERE rs.IsDeleted = 0
                                        AND RouteVarId = @routeVarId
                                        AND rs.IsDeleted = 0
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
                                                    AND t.IsDeleted = 0
                                                    AND RouteVarId = @routeVarId
                                                    AND tt.ApplyDates LIKE @today";

    /// <summary>
    /// Get Trip By 
    /// <param name="id">TripId</param>
    /// </summary>                         
    public const string GET_TRIP_BY_ID = @"SELECT t.Id,tt.ApplyDates, t.[Name], t.StartTime, t.EndTime, t.[Status] 
                                                    FROM TimeTables tt JOIN Trip t
                                                    ON t.TimeTableId = tt.Id
                                                    WHERE t.Id = @id AND t.IsDeleted = 0";

    /// <summary>
    /// Get Trips By 
    /// <param name="timeTableId">TimeTableId</param> 
    /// </summary>
    public const string GET_TRIP_BY_TIMETABLEID = @"SELECT t.Id,tt.ApplyDates, t.[Name], t.StartTime, t.EndTime, t.[Status] 
                                                    FROM TimeTables tt JOIN Trip t
                                                    ON t.TimeTableId = tt.Id
                                                    WHERE tt.Id = @timeTableId AND t.IsDeleted = 0
                                                    ORDER BY t.StartTime";

    /// <summary>
    /// Get Trip Schedule by TripId
    /// </summary>
    public const string GET_TRIP_SCHEDULE_BY_ID = @"SELECT rs.[Index] ,rs.DistanceFromStart, rs.DistanceToNext, rs.DurationFromStart, 
                                                    rs.DurationToNext, s.[Name] AS StationName,
                                                    DATEADD(MINUTE, rs.DurationFromStart, PARSE(t.StartTime AS time)) AS ArrivalTime,
                                                    rs.StationId AS StationId,
                                                    s.StoreId AS StoreId, s.Latitude, s.Longitude
                                                    FROM RouteStation rs
                                                    JOIN Station s
                                                    ON s.Id = rs.StationId 
                                                    JOIN RouteVars rv ON rs.RouteVarId = rv.Id
                                                    JOIN TimeTables tt ON tt.RouteVarId = rv.Id
                                                    JOIN Trip t ON tt.Id = t.TimeTableId
                                                    WHERE t.Id = @id
                                                    AND t.IsDeleted = 0
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
                                                WHERE tt.RouteVarId = @routeVarId
                                                AND tt.RouteId = @routeId
                                                AND tt.IsDeleted = 0
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
                                                WHERE rv.RouteId = @routeId AND rv.IsDeleted = 0
                                                ORDER BY rv.RouteVarId";
    /// <summary>
    /// CommandText của GET TIMETABLE BY ID
    /// </summary>
    public const string GET_TIMETABLE_BY_ID = @"SELECT * FROM TimeTables 
                                                WHERE Id = @id 
                                                AND t.IsDeleted = 0";
}