using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Timetables.Commands;
using PTP.Application.Features.Timetables.Queries;
using PTP.Application.ViewModels.Timetables;

namespace PTP.WebAPI.Controllers;
public class TimetablesController : BaseController
{
    private readonly IMediator mediator;
    public TimetablesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region READ
    /// <summary>
    /// Lấy Timetable theo RouteId và RouteVarId
    /// </summary>
    /// <param name="routeId">Guid</param>
    /// <param name="routeVarId">Guid</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [Route("/api/routes/{routeId}/route-vars/{routeVarId}/timetables")]
    [HttpGet]
    public async Task<IActionResult> GetTimetableByParId(Guid routeId, Guid routeVarId)
    => Ok(await mediator.Send(new GetTimetableByParIdQuery { RouteId = routeId, RouteVarId = routeVarId }));





    #endregion
    #region WRITE
    /// <summary>
    /// Delete 1 Timetable
    /// </summary>
    /// <param name="id">Guid</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await mediator.Send(new DeleteTimetableCommand { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Tạo mới 1(n) Timetable Thủ công
    /// </summary>
    /// <param name="models">List --TimeTableCreateModel </param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create(List<TimetableCreateModel> models)
    {
        var result = await mediator.Send(new CreateTimetableCommand { Models = models });
        return Ok(result);

    }

    /// <summary>
    /// Update một Timetable
    /// </summary>
    /// <param name="id">Guid</param>
    /// <param name="model">TimetableUpdateModel</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TimetableUpdateModel model)
    {
        await mediator.Send(new UpdateTimetableCommand { Id = id, Model = model });
        return NoContent();
    }

    [HttpPost, Route("{id}/trips")]
    public async Task<IActionResult> ApplyTrips(ApplyTripCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    #endregion
}