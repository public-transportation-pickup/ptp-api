using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Trips.Commands;
using PTP.Application.Features.Trips.Queries;
using PTP.Application.ViewModels.Trips;

namespace PTP.WebAPI.Controllers;
public class TripsController : BaseController
{
    private readonly IMediator mediator;
    public TripsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region READ
    /// <summary>
    /// Lấy toàn bộ trip theo RouteId và RouteVarId
    /// </summary>
    /// <param name="routeId"></param>
    /// <param name="routeVarId"></param>
    /// <param name="pageNumber"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Route("/api/routes/{routeId}/route-vars/{routeVarId}/trips")]
    [HttpGet]
    public async Task<IActionResult> GetTripsByParentId(Guid routeId, Guid routeVarId, [FromQuery] int pageNumber = 0, [FromQuery] Dictionary<string, string> filter = default!)
    => Ok(await mediator.Send(new GetAllTripByRouteIdQuery
    {
        RouteId = routeId,
        RouteVarId = routeVarId,
        Filter = filter,
        PageNumber = pageNumber
    }));


    /// <summary>
    /// Lấy toàn bộ trip theo TimeTableId
    /// </summary>
    /// <param name="id">Guid</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [Route("/api/timetables/{id}/trips")]
    [HttpGet]
    public async Task<IActionResult> GetTripsByTimeTableId([FromQuery] Guid id)
    => Ok(await mediator.Send(new GetTripsByTimeTableIdQuery { TimeTableId = id }));
    /// <summary>
    /// Get Trip By id
    /// </summary>
    /// <param name="isSchedule"> Có Include theo schedule hay không</param>
    /// <param name="id">Trip Id</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTripById(Guid id, [FromQuery] bool isSchedule = false)
    => Ok(await mediator.Send(new GetTripByIdQuery { Id = id, IsSchedule = isSchedule }));


    #endregion

    #region WRITE
    /// <summary>
    /// Cập nhật trip
    /// </summary>
    /// <param name="id">Trip Id</param>
    /// <param name="model">Model của trip Update</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TripUpdateModel model)
    {
        var result = await mediator.Send(new UpdateTripCommand { Id = id, Model = model });
        return result ? NoContent() : BadRequest();
    }


    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TripCreateModel model)
    {
        var result = await mediator.Send(new CreateTripCommand { Model = model });
        if (result is not null)
        {
            return CreatedAtAction(
                actionName: nameof(GetTripById),
                routeValues: new { id = result.Id },
                value: result);
        }
        else return BadRequest();
    }
    /// <summary>
    /// Xoá Trip
    /// </summary>
    /// <param name="id">Trip Id</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
        => await mediator.Send(new DeleteTripCommand { Id = id }) ? NoContent() : BadRequest();

    #endregion
}