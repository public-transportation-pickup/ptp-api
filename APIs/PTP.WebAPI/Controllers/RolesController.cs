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
        string fcm = "fgMcaOSnSQem8NlJ8en6sw:APA91bHKRiEZMnCvhLfgBlVtBPGCZ-zI9-6nJMnxWAABYceKIBZZHC4hT2gVNw1Uo-ttyIZqZvrsWhb6vv-VhYOO7fThksvDezlSuZLyO2x53VY3ZIYUoKOr6SX9iQjfXfqVuZbk9dnR";
        await FirebaseUtilities.SendNotification(fcm, "Test", "Test message", _appSettings.FirebaseSettings.SenderId, _appSettings.FirebaseSettings.ServerKey);
        return Ok();
    }

}