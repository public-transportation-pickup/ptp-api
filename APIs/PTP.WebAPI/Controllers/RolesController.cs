using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application;
using PTP.Application.Features.Roles.Queries;
using PTP.Application.Utilities;

namespace PTP.WebAPI.Controllers;
public class RolesController : BaseController
{
    private readonly IMediator _mediator;
    public RolesController(IMediator mediator)
    {
        _mediator = mediator;

    }

    /* 
        This only endpoint for testing
    */
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _mediator.Send(new RoleQuery()));
    }

}