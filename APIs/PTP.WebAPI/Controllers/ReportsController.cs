using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Reports.Queries;

namespace PTP.WebAPI.Controllers;

public class ReportsController : BaseController
{
    private readonly IMediator mediator;
    public ReportsController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    [HttpGet("admin")]
    public async Task<IActionResult> GetAdminReport()
    {
        return Ok(await mediator.Send(new GetAdminReportQuery()));
    }
}