using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    /// Lấy toàn bộ trạm
    /// </summary>
    /// <param name="pageNumber">Default 0</param>
    /// <param name="filter">"key" - "value"</param>
    /// Case sensitive dành cho filter, Ex: "Name" : "Trời ơi"
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int pageNumber = 0, [FromQuery] Dictionary<string, string> filter = default!)
        => Ok(await mediator.Send(new GetAllStationQuery { Filter = filter, PageNumber = pageNumber }));

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



}