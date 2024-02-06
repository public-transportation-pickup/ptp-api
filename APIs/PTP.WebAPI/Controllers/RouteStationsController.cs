using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.RouteStations.Queries;

namespace PTP.WebAPI.Controllers;
public class RouteStationsController : BaseController 
{
    private readonly IMediator mediator;
    public RouteStationsController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    #region READ
    [Route("/api/routes/{id}/route-vars/{routeVarId}/route-stations")]
    [HttpGet]
    public async Task<IActionResult> GetRouteStationByParId(Guid id, Guid routeVarId)
    => Ok(await mediator.Send(new GetRouteStationByParIdQuery {Id = id, RouteVarId = routeVarId}));
    #endregion
}