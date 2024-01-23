using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Orders.Queries;
using PTP.Application.Features.Users.Commands;
using PTP.Application.Features.Users.Queries;
using PTP.Application.ViewModels.Users;

namespace PTP.WebAPI.Controllers;
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #region  READ
    /// <summary>
    /// Lấy hết tất cả User
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Get()
    => Ok(await _mediator.Send(new GetAllUserQuery()));

    /// <summary>
    /// Lấy User theo Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    => Ok(await _mediator.Send(new GetUserByIdQuery { Id = id }));

    /// <summary>
    /// Lấy order theo UserId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}/orders")]
    public async Task<IActionResult> GetOrderByUserId(Guid id)
    => Ok(await _mediator.Send(new GetOrdersByUserIdQuery { UserId = id }));
    #endregion
    #region WRITE

    /// <summary>
    /// Tạo mới một user, thủ công, Admin using
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateModel model)
    {
        var result = await _mediator.Send(new CreateUserCommand { Model = model });
        if (result is not null)
        {
            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = result.Id },
                value: result
            );
        }
        else return BadRequest();
    }

    /// <summary>
    /// Xoá User theo Id - Admin
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteUserCommand { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Cập nhật user, cập nhật profile
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateModel model)
    {
        await _mediator.Send(new UpdateUserCommand { Model = model, Id = id });
        return NoContent();
    }
    #endregion
}