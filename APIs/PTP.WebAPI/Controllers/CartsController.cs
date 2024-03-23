using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PTP.Application.Features.Carts.Commands;
using PTP.Application.Features.Carts.Queries;



namespace PTP.WebAPI.Controllers;
[Route("api/[controller]")]
public class CartsController : BaseController
{
    private readonly IMediator mediator;
    public CartsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        return Ok(await mediator.Send(new GetCartByUserQuery()));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCart(CreateCartCommand request)
    {
        return Ok(await mediator.Send(request));
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteCart(DeleteCartCommand request)
        => Ok(await mediator.Send(request));

    [HttpPut, Authorize]
    public async Task<IActionResult> UpdateCart(UpdateCartCommand request)
    {
        return Ok(await mediator.Send(request));
    }
}