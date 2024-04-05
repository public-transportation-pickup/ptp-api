using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application;
using PTP.Application.Features.Roles.Queries;
using PTP.Application.Utilities;

namespace PTP.WebAPI.Controllers;
public class RolesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly AppSettings _appSettings;
    public RolesController(IMediator mediator, AppSettings appSettings)
    {
        _mediator = mediator;
        _appSettings = appSettings;

    }

    /* 
        This only endpoint for testing
    */
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _mediator.Send(new RoleQuery()));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> TestNotify(int id)
    {
        string fcm = "coYahtu78b2ZSNKo0sF-cj:APA91bGwxSoNAA-vAzxm2nG0Zxt7NSZHdpsNdWgQVQzx6soc1k3iopzAC9sdrzL3RuuECxRh19ndDc2eYbjjxxPl59W4oZ_AexCPVJo3yNSTHM0gX0fBpkA2f2DslCGOk5i5D-Y-qvEb";
        await FirebaseUtilities.SendNotification(fcm, "Test", "Test message", _appSettings.FirebaseSettings.SenderId, _appSettings.FirebaseSettings.ServerKey);
        return Ok();
    }

}