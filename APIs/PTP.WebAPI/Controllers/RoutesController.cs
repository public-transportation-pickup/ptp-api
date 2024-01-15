using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Routes.Commands;
using PTP.Application.Features.Routes.Queries;
using PTP.Application.IntergrationServices.Interfaces;

namespace PTP.WebAPI.Controllers;
public class RoutesController : BaseController
{
	// private readonly IBusRouteService _intergrationBus;
	// public RoutesController(IBusRouteService busRouteService)
	// {
	//     _intergrationBus = busRouteService;
	// }
	// [ProducesResponseType((int)HttpStatusCode.Created)]
	// [ProducesResponseType((int)HttpStatusCode.BadRequest)]
	// [HttpPost]
	// public async Task<IActionResult> Post()
	// {
	//     await _intergrationBus.CheckNewCreatedRoute();
	//    return Ok();
	// } 

	private readonly IMediator _mediator;
	public RoutesController(IMediator mediator)
	{
		_mediator = mediator;
	}

	#region READ 
	[ProducesResponseType((int)HttpStatusCode.OK)]
	[ProducesResponseType((int)HttpStatusCode.BadRequest)]
	[HttpGet]
	public async Task<IActionResult> Get() => Ok(await _mediator.Send(new GetAllRouteQuery()));

    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    => Ok(await _mediator.Send(new GetRouteByIdQuery { Id = id }));
    #endregion
    #region Write
    [Route("{id}/distance-modification")]
    [HttpPut]
    public async Task<IActionResult> Update([FromRoute] Guid id)
    {
        await _mediator.Send(new DistanceModificationCommand { Id = id});
        return NoContent();
    }
    #endregion

}