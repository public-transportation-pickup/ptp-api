using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Routes.Queries;
using PTP.Application.Features.Stations.Commands;
using PTP.Application.Features.Stations.Queries;
using PTP.Application.ViewModels.Stations;

namespace PTP.WebAPI.Controllers;
public class StationsController : BaseController
{
    private readonly IMediator mediator;
    public StationsController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    /// <summary>
    /// Get Routes bằng stationName
    /// </summary>
    /// <param name="stationName"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet("routes")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetRouteByStation([FromQuery] string stationName,
        int? pageNumber,
        int? pageSize)
    {
        return Ok(await mediator.Send(new GetRouteByStationQuery { StationName = stationName, PageNumber = pageNumber, PageSize = pageSize }));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue([FromQuery] Dictionary<string, string> filters)
    {
        return Ok(await mediator.Send(new GetStationRevenueQuery() { Filter = filters }));
    }
    /// <summary>
    /// Lấy toàn bộ trạm
    /// </summary>
    /// <param name="pageNumber">Default 0</param>
    /// <param name="filter">"key" - "value"</param>
    /// <param name="pageSize">Default 100</param>
    /// Case sensitive dành cho filter, Ex: "Name" : "Trời ơi"
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Dictionary<string, string> filter, [FromQuery] int pageNumber = -1,
        [FromQuery] int pageSize = 100)
        => Ok(await mediator.Send(new GetAllStationQuery { Filter = filter, PageNumber = pageNumber, PageSize = pageSize }));

    /// <summary>
    ///  Lấy thông tin một trạm theo Id
    /// </summary>
    /// <param name="id">Guid</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetById(Guid id)
    => Ok(await mediator.Send(new GetStationByIdQuery { Id = id }));


    /// <summary>
    /// Xoá một trạm - unuse
    /// </summary>
    /// <param name="id">Guid</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await mediator.Send(new DeleteStationCommand { Id = id });
        return NoContent();
    }
    /// <summary>
    /// Update thông tin của một trạm
    /// </summary>
    /// <param name="id">Guid</param>
    /// <param name="model"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] StationUpdateModel model)
    {
        await mediator.Send(new UpdateStationCommand { Id = id, Model = model });
        return NoContent();
    }


    /// <summary>
    /// Đăng ký store đến một station
    /// </summary>
    /// <param name="id"></param>
    /// <param name="stationId"></param>
    /// <returns></returns>
    [HttpPut("/api/stores/{id}/stations/{stationId}")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> AddStore([FromRoute] Guid id, [FromRoute] Guid stationId)
    {
        await mediator.Send(new AddStoreIntoStationCommand { StationId = stationId, StoreId = id });
        return NoContent();
    }


    /// <summary>
    /// Lấy hết station theo RouteVarId có xếp theo Index
    /// </summary>
    /// <param name="routeVarId"></param>
    /// <returns></returns>
    [HttpGet("/api/route-vars/{routeVarId}/stations")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetStationByRouteVarId(Guid routeVarId)
    {
        return Ok(await mediator.Send(new GetAllStationByRouteIdQuery { Id = routeVarId }));
    }

}