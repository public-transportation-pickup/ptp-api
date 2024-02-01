using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Wallets.Commands;
using PTP.Application.Features.Wallets.Queries;
using PTP.Application.ViewModels.Wallets;

namespace PTP.WebAPI.Controllers;
public class WalletsController : BaseController
{
    private readonly IMediator mediator;
    public WalletsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region Queries

    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWalletById([FromRoute] Guid id)
    => Ok(await mediator.Send(new GetWalletByIdQuery { Id = id }));
    #endregion

    /// <summary>
    ///  Nạp tiền vào ví
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> AddAccountBalance([FromBody] AddAccountBalanceModel model)
    {
        var result = await mediator.Send(new AddAccountBalanceCommand { Amount = model.Amount, Source = model.Source, Type = model.Type });
        if(result)
        {
            return StatusCode(201);
        } else return BadRequest();
    }
}
