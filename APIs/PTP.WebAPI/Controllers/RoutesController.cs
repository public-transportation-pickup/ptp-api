using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _mediator.Send(new GetAllRouteQuery()));
    #endregion

}