using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Menus.Commands;
using PTP.Application.Features.Menus.Queries;
using PTP.Application.Features.ProductMenus.Queries;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Enums;

namespace PTP.WebAPI.Controllers;

public class MenusController : BaseController
{
    public readonly IMediator _mediator;

    public MenusController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Queries
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _mediator.Send(new GetAllMenuQuery()));



    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,StoreManager")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    => Ok(await _mediator.Send(new GetMenuByIdQuery { Id = id }));


    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{menuid}/products-menu")]
    [Authorize(Roles = "Admin,StoreManager")]
    public async Task<IActionResult> GetProductMenuByMenuId([FromRoute] Guid menuid,
                                                            [FromQuery] Dictionary<string, string> filter,
                                                            [FromQuery] int pageNumber = 0,
                                                            [FromQuery] int pageSize = 10)

    => Ok(await _mediator.Send(new GetProductInMenuByMenuIdQuery { MenuId = menuid, PageNumber = pageNumber, PageSize = pageSize, Filter = filter }));

    #endregion 

    #region Commands
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost]
    //[Authorize(Roles = (nameof(RoleEnum.StoreManager)))]
    public async Task<IActionResult> Create(MenuCreateModel model)
    {
        var result = await _mediator.Send(new CreateMenuCommand { CreateModel = model });
        if (result is null)
        {
            return BadRequest("Create Fail!");
        }
        return StatusCode(201, result);
    }



    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPut("{id}")]
    [Authorize(Roles = (nameof(RoleEnum.StoreManager)))]
    public async Task<IActionResult> Update(Guid id, MenuUpdateModel model)
    {
        if (id != model.Id) return BadRequest("Id is not match!");
        var result = await _mediator.Send(new UpdateMenuCommand { UpdateModel = model });
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
    [Authorize(Roles = (nameof(RoleEnum.StoreManager)))]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteMenuCommand { Id = id });
        if (!result)
        {
            return BadRequest("Delete Fail!");
        }
        return NoContent();
    }
    #endregion
}