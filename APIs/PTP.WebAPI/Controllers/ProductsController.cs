using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Products.Commands;
using PTP.Application.Features.Products.Queries;
using PTP.Application.ViewModels.Products;
using PTP.Domain.Enums;

namespace PTP.WebAPI.Controllers;
public class ProductsController : BaseController
{
    public readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Queries
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Get(
                                        [FromQuery] Dictionary<string, string> filter,
                                        [FromQuery] int pageNumber = 0,
                                        [FromQuery] int pageSize = 10)
    => Ok(await _mediator.Send(new GetAllProductQuery { PageNumber = pageNumber, PageSize = pageSize, Filter = filter }));



    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    => Ok(await _mediator.Send(new GetProductByQuery { Id = id }));

    #endregion 

    #region Commands
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost]
    [Authorize(Roles = (nameof(RoleEnum.StoreManager)))]
    public async Task<IActionResult> Create([FromForm] ProductCreateModel model)
    {
        var result = await _mediator.Send(new CreateProductCommand { CreateModel = model });
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
    [Authorize(Roles = (nameof(RoleEnum.StoreManager)))]
    public async Task<IActionResult> Update(Guid id, [FromForm] ProductUpdateModel model)
    {
        if (id != model.Id) return BadRequest("Id is not match!");
        var result = await _mediator.Send(new UpdateProductCommand { UpdateModel = model });
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
        var result = await _mediator.Send(new DeleteProductCommand { Id = id });
        if (!result)
        {
            return BadRequest("Delete Fail!");
        }
        return NoContent();
    }
    #endregion
}