using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Categories.Queries;
using PTP.Application.Features.Products.Queries;
using PTP.Application.Features.Wallets.Queries;
using System.Net;

namespace PTP.WebAPI.Controllers
{
    public class WalletsController:BaseController
    {
        public readonly IMediator _mediator;

        public WalletsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Queries

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWalletById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetWalletByIdQuery { Id = id }));
        #endregion
    }
}
