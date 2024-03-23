using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.RouteVars.Commands;
using PTP.Application.Features.RouteVars.Queries;
using PTP.Application.ViewModels.RouteStations;
using PTP.Application.ViewModels.RouteVars;
using PTP.Application.ViewModels.Stations;

namespace PTP.WebAPI.Controllers;
public class RouteVarsController : BaseController
{
    private readonly IMediator _mediator;
    public RouteVarsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #region READ
    /// <summary>
    /// Lấy Tất cả routeVar theo RouteId
    /// </summary>
    /// <param name="routeId">Guid</param>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/routes/{routeId}/route-vars")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAllRouteVar(Guid routeId)
    {
        var result = await _mediator.Send(new GetAllRouteVarByRouteIdQuery { RouteId = routeId });
        if (result is not null && result.Any())
        {
            return Ok(result);
        }
        else return Ok();
    }

    /// <summary>
    /// Lấy 1 RouteVar theo Id
    /// </summary>
    /// <param name="id">Guid</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetRouteVarByIdQuery { Id = id });
        return result is null ? BadRequest() : Ok(result);
    }
    #endregion

    #region  WRITE
    /// <summary>
    /// Xoá Route Variation
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteRouteVarCommand { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Tạo mới một RouteVariation thủ công - No Longer Use
    /// </summary>
    /// <param name="model">RouteVarCreateModel</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RouteVarCreateModel model)
    {
        var result = await _mediator.Send(new CreateRouteVarCommand { Model = model });
        if (result is not null)
        {
            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = result.Id },
                value: result);
        }
        else return BadRequest();
    }
    #endregion
    /// <summary>
    /// Duplicate một routeVar, thay đổi tuyến đường
    /// </summary>
    /// <param name="id"></param>
    /// <param name="models"></param>
    /// <returns></returns>
    [Route("{id}/duplicate")]
    [HttpPost]
    public async Task<IActionResult> Duplicate([FromRoute] Guid id, [FromBody] List<RouteStationDuplicateModel> models)
    {
        return Ok(await _mediator.Send(new DuplicateRouteVarCommand { Id = id, Stations = models }));
    }

    /// <summary>
    /// Load tất cả các store theo routeVariation
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/stores")]
    public async Task<IActionResult> GetStoreByRouteVars([FromRoute] Guid id)
    {
        return Ok(await _mediator.Send(new GetStoresByRouteVarId { RouteVarId = id }));
    }
}
