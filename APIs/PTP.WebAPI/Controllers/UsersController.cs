using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Orders.Queries;
using PTP.Application.Features.Products.Queries;
using PTP.Application.Features.Stores.Queries;
using PTP.Application.Features.Trips.Queries;
using PTP.Application.Features.Users.Commands;
using PTP.Application.Features.Users.Queries;
using PTP.Application.Features.Wallets.Queries;
using PTP.Application.ViewModels.Users;
using PTP.Domain.Enums;

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
    public async Task<IActionResult> Get([FromQuery] Dictionary<string, string> filter,
        [FromQuery] int pageNumber = -1)
    => Ok(await _mediator.Send(new GetAllUserQuery
    {
        Filter = filter,
        PageNumber = pageNumber
    }));
    /// <summary>
    /// Lấy user từ claims
    /// </summary>
    /// <returns></returns>
    [HttpGet("details")]
    public async Task<IActionResult> GetUserByToken(GetUserByTokenQuery query)
    => Ok(await _mediator.Send(query));



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
    /// Lấy Wallet theo UserId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}/wallets")]
    public async Task<IActionResult> GetWallettByUserId(Guid id)
    => Ok(await _mediator.Send(new GetWalletByUserIdQuery { UserId = id }));

    /// <summary>
    /// Lấy Store theo UserId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Roles = nameof(RoleEnum.StoreManager))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}/stores")]
    public async Task<IActionResult> GetStoreByUserId(Guid id)
    => Ok(await _mediator.Send(new GetStoreByUserIdQuery { UserId = id }));


    /// <summary>
    /// Lấy order theo UserId
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("orders")]
    public async Task<IActionResult> GetOrderByUserId(
                                                    [FromQuery] int pageNumber = -1,
                                                    [FromQuery] int pageSize = 100,
                                                    [FromQuery] Dictionary<string, string> filter = default!)
    => Ok(await _mediator.Send(new GetOrdersByUserIdQuery { PageNumber = pageNumber, PageSize = pageSize, Filter = filter }));
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

    /// <summary>
    /// Dự đoán chuyến theo current location của user
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost("trips/coordinate")]
    public async Task<IActionResult> GetTripByCoordinate(GetTripByUserLocation query)
    {
        return Ok(await _mediator.Send(query));
    }

    /// <summary>
    /// Lấy các order quen, top products theo user
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("top-products")]
    public async Task<IActionResult> GetTopProducts()
     => Ok(await _mediator.Send(new GetTopProductQuery()));
}