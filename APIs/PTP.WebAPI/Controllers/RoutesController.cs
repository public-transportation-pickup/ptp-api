using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Routes.Commands;
using PTP.Application.Features.Routes.Queries;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Application.ViewModels.Routes;

namespace PTP.WebAPI.Controllers;
public class RoutesController : BaseController
{

	private readonly IMediator _mediator;
	public RoutesController(IMediator mediator)
	{
		_mediator = mediator;
	}
	/// <summary>
	/// Lấy tất cả Route 
	/// </summary>
	/// <returns></returns>
	#region READ 
	[ProducesResponseType((int)HttpStatusCode.OK)]
	[ProducesResponseType((int)HttpStatusCode.BadRequest)]
	[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
	[HttpGet]
	public async Task<IActionResult> Get(
	
		[FromQuery] Dictionary<string, string> filter = default!,
		[FromQuery] int pageNumber = 0) 
			=> Ok(await _mediator.Send(new GetAllRouteQuery 
			{
				Filter = filter,
				PageNumber = pageNumber
			}));
	/// <summary>
	/// Lấy Route theo Id
	/// </summary>
	/// <param name="id">Guid</param>
	/// <returns></returns>

	[ProducesResponseType((int)HttpStatusCode.OK)]
	[ProducesResponseType((int)HttpStatusCode.BadRequest)]
	[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] Guid id)
	=> Ok(await _mediator.Send(new GetRouteByIdQuery { Id = id }));
	#endregion
	#region Write
	/// <summary>
	/// Cập nhật Distance và duration cho một route, chỉ gọi khi cần, API này call ra ngoài nhiều 
	/// Note: Distance to Start và Duration to Start chưa chính xác, chỉ đúng trường hợp tuyến là 1 đường thẳng
	/// </summary>
	/// <param name="id">Guid</param>
	/// <returns></returns>
	[Route("{id}/distance-modification")]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.BadRequest)]
	[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
	[HttpPut]
	public async Task<IActionResult> Update([FromRoute] Guid id)
	{
		await _mediator.Send(new DistanceModificationCommand { Id = id });
		return NoContent();
	}
	/// <summary>
	/// Xoá một route
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.BadRequest)]
	[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		return await _mediator.Send(new DeleteRouteCommand { Id = id }) ? NoContent() : BadRequest();
	}
	/// <summary>
	/// Update một route
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <returns></returns>
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType((int)HttpStatusCode.BadRequest)]
	[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
	[HttpPut("{id}")]
	public async Task<IActionResult> Update(Guid id, [FromBody] RouteUpdateModel model)
		=> await _mediator.Send(new UpdateRouteCommand { Id = id, Model = model }) ? NoContent() : BadRequest();
	#endregion

}