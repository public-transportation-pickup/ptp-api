
using System.Threading;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Users.Queries;
using PTP.Application.Features.Wallets.Commands;
using PTP.Application.Features.Wallets.Queries;
using PTP.Application.IntergrationServices.Models;
using PTP.Application.ViewModels.Wallets;
using System.Net;

namespace PTP.WebAPI.Controllers;
public class WalletsController : BaseController
{
	private readonly IMediator mediator;
	public WalletsController(IMediator mediator)
	{
		this.mediator = mediator;
	}
	[HttpGet("test")]
	public IActionResult Test()
	{
		var html = System.IO.File.ReadAllText(@"./wwwroot/payment-sucess.html");
		return base.Content(html, "text/html");
	}
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
		if (result)
		{
			return StatusCode(201);
		}
		else return BadRequest();
	}
	[HttpGet("vn-pay/response")]
	public async Task<IActionResult> VNPayResponse([FromQuery] VnPayResponseModel model)
	{
		string html = string.Empty;
		var result = await mediator.Send(new VnPayResponseCommand { Model = model });
		var user = await mediator.Send(new GetUserByIdQuery { Id = model.userId });
		if (result)
		{
			html = System.IO.File.ReadAllText(@"./wwwroot/payment-sucess.html")
				.Replace("{{User}}", user.Email)
				.Replace("{{Amount}}", (model.vnp_Amount / 100).ToString())
				.Replace("{{CreateDate}}", DateTime.Now.ToString("dd/MM/yyyy"));
			return base.Content(html, "text/html");

		}
		else
		{
			html = System.IO.File.ReadAllText(@"./wwwroot/payment-fail.html")
							.Replace("{{User}}", user.Email);
			return base.Content(html, "text/html");
		}

	}
	[Authorize]
	[HttpPost("vn-pay")]
	public async Task<IActionResult> VNPayCallBack([FromBody] decimal amount)
	{
		var result = await mediator.Send(new RequestVNPayCommand { Amount = amount });
		return Ok(result);
	}

	[Authorize]
	[HttpPost("vn-pay/refund")]
	public async Task<IActionResult> VNPayCallBackRefund([FromBody] string TxnRef)
	{
		var result = await mediator.Send(new RequestRefundVNPayCommand { TxnRef = TxnRef });
		return Ok(result);
	}
	#region Queries

	[ProducesResponseType((int)HttpStatusCode.OK)]
	[ProducesResponseType((int)HttpStatusCode.BadRequest)]
	[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
	[HttpGet("{id}")]
	public async Task<IActionResult> GetWalletById([FromRoute] Guid id)
	=> Ok(await mediator.Send(new GetWalletByIdQuery { Id = id }));
	#endregion
}



