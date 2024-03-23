using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PTP.Application.Features.Menus.Queries;
using PTP.Application.Features.Orders.Queries;
using PTP.Application.Features.Products.Queries;
using PTP.Application.Features.Stores.Commands;
using PTP.Application.Features.Stores.Queries;
using PTP.Application.ViewModels.Stores;
using PTP.Domain.Enums;
using System.Net;

namespace PTP.WebAPI.Controllers
{
	public class StoresController : BaseController
	{
		private readonly IMediator _mediator;

		public StoresController(IMediator mediator)
		{
			_mediator = mediator;
		}
		#region QUERIES
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[HttpGet]
		public async Task<IActionResult> Get(
											[FromQuery] Dictionary<string, string> filter,
											[FromQuery] int pageNumber = 0,
											[FromQuery] int pageSize = 10)
		=> Ok(await _mediator.Send(new GetAllStoreQuery { Filter = filter, PageNumber = pageNumber, PageSize = pageSize }));



		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		=> Ok(await _mediator.Send(new GetStoreByIdQuery { Id = id }));

		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[HttpGet("{id}/menus")]
		public async Task<IActionResult> GetMenusByStoreId([FromRoute] Guid id, string? arrivalTime, DateTime? dateApply)
		{
			if (arrivalTime.IsNullOrEmpty()) return Ok(await _mediator.Send(new GetMenusByStoreId { StoreId = id }));

			return Ok(await _mediator.Send(new GetMenuDetailByStoreId { StoreId = id, ArrivalTime = arrivalTime!, DateApply = dateApply!.Value }));
		}


		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[HttpGet("{id}/products")]
		[Authorize(Roles = "Admin,StoreManager")]
		public async Task<IActionResult> GetProductsByStoreId([FromRoute] Guid id,
															  //   [FromQuery] Guid categoryId = default,
															  [FromQuery] int pageNumber = 0,
															  [FromQuery] int pageSize = 10,
															  [FromQuery] Dictionary<string, string> filter = default!)
		// => Ok(await _mediator.Send(new GetProductsByStoreIdQuery { StoreId = id, CategoryId = categoryId,PageNumber=pageNumber,PageSize=pageSize,Filter=filter }));
		=> Ok(await _mediator.Send(new GetProductsByStoreIdQuery { StoreId = id, PageNumber = pageNumber, PageSize = pageSize, Filter = filter }));

		// [ProducesResponseType((int)HttpStatusCode.OK)]
		// [ProducesResponseType((int)HttpStatusCode.BadRequest)]
		// [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		// [HttpGet("{id}/stations")]
		// public async Task<IActionResult> GetStationsStoreId([FromRoute] Guid id)
		// => Ok("Comming soon!");

		// [ProducesResponseType((int)HttpStatusCode.OK)]
		// [ProducesResponseType((int)HttpStatusCode.BadRequest)]
		// [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		// [HttpGet("{id}/wallets")]
		// public async Task<IActionResult> GetWalletByStoreId([FromRoute] Guid id)
		// => Ok(await _mediator.Send(new GetWalletByStoreIdQuery { StoreId = id }));

		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[HttpGet("{id}/orders")]
		[Authorize(Roles = "Admin,StoreManager")]
		public async Task<IActionResult> GetOrdersByStoreId([FromRoute] Guid id,
															[FromQuery] int pageNumber = 0,
															[FromQuery] int pageSize = 10,
															[FromQuery] Dictionary<string, string> filter = default!,
															[FromQuery] string roleName = default!)
		=> Ok(await _mediator.Send(new GetOrdersByStoreIdQuery { StoreId = id, PageSize = pageSize, PageNumber = pageNumber, Filter = filter, RoleName = roleName }));
		#endregion

		#region COMMANDS


		[ProducesResponseType((int)HttpStatusCode.Created)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[HttpPost]
		//[Authorize(Roles = (nameof(RoleEnum.Admin)))]
		public async Task<IActionResult> Create([FromForm] StoreCreateModel model)
		{
			var result = await _mediator.Send(new CreateStoreCommand { CreateModel = model });
			if (result is null)
			{
				return BadRequest("Create Fail!");
			}
			return CreatedAtAction(nameof(GetById), new { Id = result.Id }, result);
		}



		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[HttpPut("{id}")]
		[Authorize(Roles = (nameof(RoleEnum.Admin)))]
		public async Task<IActionResult> Update(Guid id, [FromForm] StoreUpdateModel model)
		{
			if (id != model.Id) return BadRequest("Id is not match!");
			var result = await _mediator.Send(new UpdateStoreCommand { StoreUpdate = model });
			if (!result)
			{
				return BadRequest("Update Fail!");
			}
			return NoContent();
		}

		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[HttpDelete("{id}")]
		[Authorize(Roles = (nameof(RoleEnum.Admin)))]
		public async Task<IActionResult> Delete(Guid id)
		{
			var result = await _mediator.Send(new DeleteStoreCommand { Id = id });
			if (!result)
			{
				return BadRequest("Delete Fail!");
			}
			return NoContent();
		}


		#endregion

	}
}
