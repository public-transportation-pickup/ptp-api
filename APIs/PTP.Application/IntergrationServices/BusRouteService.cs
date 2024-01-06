using Newtonsoft.Json;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Application.IntergrationServices.Models;
using PTP.Domain.Entities;
using PTP.Domain.Enums;

namespace PTP.Application.IntergrationServices;
public class BusRouteService : IBusRouteService
{
    private readonly HttpClient httpClient = new();
    private readonly IUnitOfWork _unitOfWork;
    public BusRouteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> TryCreateRouteAsync(int routeId)
    {
        var isDup = await _unitOfWork.RouteRepository.FirstOrDefaultAsync(x => x.RouteId == routeId);
        if (isDup is not null) return false;
        #region 1. Add Route
        RouteModel routeModel = await getRouteAsync(routeId);
        var route = new Route
        {
            Id = Guid.NewGuid(),
            Distance = routeModel.Distance,
            HeadWay = routeModel.Headway ?? string.Empty,
            InBoundDescription = routeModel.InBoundDescription ?? string.Empty,
            OutBoundDescription = routeModel.OutBoundDescription ?? string.Empty,
            InBoundName = routeModel.InBoundName ?? string.Empty,
            OutBoundName = routeModel.OutBoundName ?? string.Empty,
            Name = routeModel.RouteName ?? string.Empty,
            //RouteId = routeModel.RouteId,
            RouteNo = routeModel.RouteNo ?? string.Empty,
            Tickets = routeModel.Tickets ?? string.Empty,
            NumOfSeats = routeModel.NumOfSeats ?? string.Empty,
            Orgs = routeModel.Orgs ?? string.Empty,
            OperationTime = routeModel.OperationTime ?? string.Empty,
            RouteType = routeModel.Type ?? string.Empty,
            TimeOfTrip = routeModel.TimeOfTrip ?? string.Empty,
            TotalTrip = routeModel.TotalTrip ?? string.Empty,
            Status = nameof(DefaultStatusEnum.Active),
        };
        await _unitOfWork.RouteRepository.AddAsync(route);
        #endregion
        #region  2. Add Route Var
        var routeVarModels = await getRouteVarModelsAsync(routeId);
        var routeVar = routeVarModels.ConvertAll(x => new RouteVar
        {
            Id = Guid.NewGuid(),
            EndStop = x.EndStop,
            OutBound = x.OutBound,
            RouteId = route.Id,
            RouteVarId = x.RouteVarId,
            RouteVarName = x.RouteVarName,
            RouteVarShortName = x.RouteVarShortName,
            StartStop = x.StartStop,
            RunningTime = x.RunningTime,
        });
        await _unitOfWork.RouteVarRepository.AddRangeAsync(routeVar);
        await _unitOfWork.SaveChangesAsync();
        #endregion
        #region 3. Add TimeTable
        var timeTableModels = await GetTimeTableModelsAsync(routeId);
        var timeTables = timeTableModels.ConvertAll(x => new TimeTable
        {
            Id = Guid.NewGuid(),
            RouteVarId = routeVar.First(y => y.RouteVarId == x.RouteVarId).Id,
            ApplyDates = x.ApplyDates,
            TimeTableId = x.TimeTableId,
            //StartDate = DateTime.Parse(x.StartDate),
            IsCurrent = x.IsCurrent,
        });
        await _unitOfWork.TimeTableRepository.AddRangeAsync(timeTables);
        await _unitOfWork.SaveChangesAsync();
        #endregion

        #region 4. Add Trip 
        try
        {
            foreach (var timeTable in timeTables)
            {
                var tripModels = await getTripModels(routeId, timeTable.TimeTableId);
                var timeTablee = await _unitOfWork.TimeTableRepository.FirstOrDefaultAsync(x => x.TimeTableId == timeTable.TimeTableId) ?? throw new Exception($"Error At {timeTable.TimeTableId} || {timeTable.Id}");
                var trips = tripModels.ConvertAll(x => new Trip
                {
                    TimeTableId = timeTablee.Id,
                    EndTime = x.EndTime,
                    Description = string.Empty,
                    StartTime = x.StartTime,
                    Name = $""
                });
                await _unitOfWork.TripRepository.AddRangeAsync(trips);
            }
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            
        }



        #endregion

        #region Add Station and Routestation If Not exists
        foreach (var routeV in routeVar)
        {
            var stopModels = await GetStopModelsAsync(routeId, routeV.RouteVarId);
            // Check station has exist or not
            var repStopModels = new List<StopModel>();

            foreach (var stop in stopModels)
            {
                var stopIsDup = await _unitOfWork.StationRepository.FirstOrDefaultAsync(x => x.StopId == stop.StopId);
                if (stopIsDup is not null)
                {
                    await _unitOfWork.RouteStationRepository.AddAsync(new RouteStation
                    {
                        StationId = stopIsDup.Id,
                        RouteId = route.Id
                    });
                }
                else repStopModels.Add(stop);
            }
            if (repStopModels.Count > 0)
            {
                var stops = repStopModels.ConvertAll(x => new Station
                {
                    StopId = x.StopId,
                    Code = x.Code,
                    Name = x.Name ?? string.Empty,
                    StopType = x.StopType,
                    Zone = x.Zone,
                    Ward = x.Ward ?? string.Empty,
                    AddressNo = x.AddressNo,
                    Street = x.Street,
                    SupportDisability = x.SupportDisability,
                    Latitude = x.Lat,
                    Longitude = x.Lng,
                    Address = $"{x.AddressNo}, {x.Street}, {x.Ward}, {x.Zone}",
                    Status = x.Status,
                });
                await _unitOfWork.StationRepository.AddRangeAsync(stops);
                await _unitOfWork.SaveChangesAsync();
                foreach (var stop in stops)
                {
                    await _unitOfWork.RouteStationRepository.AddAsync(new RouteStation
                    {
                        StationId = stop.Id,
                        RouteId = route.Id
                    });
                }
                await _unitOfWork.SaveChangesAsync();
            }


        }
        #endregion

        return true;

    }
    private async Task<RouteModel> getRouteAsync(int routeId)
    {
        using var response = await httpClient.GetAsync($"http://apicms.ebms.vn/businfo/getroutebyid/{routeId}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        RouteModel routeModel = JsonConvert.DeserializeObject<RouteModel>(responseBody)
                                ?? throw new Exception($"Deserialize Failed for object! {responseBody}");
        return routeModel;
    }
    private async Task<List<TripModel>> getTripModels(int routeId, int timeTableId)
    {
        using var response = await httpClient.GetAsync($"http://apicms.ebms.vn/businfo/gettripsbytimetable/{routeId}/{timeTableId}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        List<TripModel> trips = JsonConvert.DeserializeObject<List<TripModel>>(responseBody) ?? throw new Exception($"Deserialize Failed for object: {responseBody}");
        return trips;

    }
    private async Task<List<RouteVarModel>> getRouteVarModelsAsync(int routeId)
    {
        using var response = await httpClient.GetAsync($"http://apicms.ebms.vn/businfo/getvarsbyroute/{routeId}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        List<RouteVarModel> routevars = JsonConvert.DeserializeObject<List<RouteVarModel>>(responseBody) ?? throw new Exception($"Deserialize Failed for object: {responseBody}");
        return routevars;
    }

    private async Task<List<TimeTableModel>> GetTimeTableModelsAsync(int routeId)
    {
        using var response = await httpClient.GetAsync($"http://apicms.ebms.vn/businfo/gettimetablebyroute/{routeId}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        List<TimeTableModel> timetables = JsonConvert.DeserializeObject<List<TimeTableModel>>(responseBody) ?? throw new Exception($"Deserialize Failed for object: {responseBody}");
        return timetables;
    }
    private async Task<List<StopModel>> GetStopModelsAsync(int routeId, int routeVarId)
    {
        using var response = await httpClient.GetAsync($"http://apicms.ebms.vn/businfo/getstopsbyvar/{routeId}/{routeVarId}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        List<StopModel> stopsModel = JsonConvert.DeserializeObject<List<StopModel>>(responseBody) ?? throw new Exception($"Deserialize Failed for object: {responseBody}");
        return stopsModel;
    }

    public async Task CheckNewCreatedRoute()
    {
        using var response = await httpClient.GetAsync($"http://apicms.ebms.vn/businfo/getallroute");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        List<RouteModel> routeModels = JsonConvert.DeserializeObject<List<RouteModel>>(responseBody)
                                ?? throw new Exception($"Deserialize Failed for object! {responseBody}");
        var routes = await _unitOfWork.RouteRepository.GetAllAsync();
        if (routes.Count == 0 || routes.Count < routeModels.Count)
        {
            foreach (var route in routeModels)
            {
                await TryCreateRouteAsync(route.RouteId);
            }
        }
        httpClient.Dispose();


    }
}
