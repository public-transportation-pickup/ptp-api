using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Trips.Queries;

namespace PTP.WebAPI.Controllers;
public class TripsController : BaseController
{
    private readonly IMediator mediator;
    public TripsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region READ
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Route("/api/routes/{routeId}/route-vars/{routeVarId}/trips")]
    [HttpGet]
    public async Task<IActionResult> GetTripsByParentId(Guid routeId, Guid routeVarId)
    => Ok(await mediator.Send(new GetAllTripByRouteIdQuery { RouteId = routeId, RouteVarId = routeVarId }));

    /// <summary>
    /// Get Trip By id
    /// </summary>
    /// <param name="id">Trip Id</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTripById(Guid id, [FromQuery] bool isSchedule= false)
    => Ok(await mediator.Send(new GetTripByIdQuery { Id = id , IsSchedule = isSchedule}));


    #endregion
}