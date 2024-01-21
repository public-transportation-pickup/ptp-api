using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.ProductMenus.Commands;
using PTP.Application.Features.ProductMenus.Queries;
using PTP.Application.ViewModels.ProductMenus;

namespace PTP.WebAPI.Controllers;


[ApiController]
[Route("api/Products-Menu")]
public class ProductInMenuController:ControllerBase
{
    
    public readonly IMediator _mediator;

    public ProductInMenuController(IMediator mediator)
    {
        _mediator=mediator;
    }

    #region Queries
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    => Ok(await _mediator.Send(new GetProductMenuByIdQuery { Id = id }));

    #endregion 

    #region Commands
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create(ProductMenuCreateModel model){
        var result=  await _mediator.Send(new CreateProductMenuCommand { CreateModel=model });
        if(result is null){
            return BadRequest("Create Fail!");
        }
        return CreatedAtAction(nameof(GetById),new {Id=result.Id},result);
    }
 


    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,ProductMenuUpdateModel model){
        if(id!=model.Id) return BadRequest("Id is not match!");
        var result=  await _mediator.Send(new UpdateProductMenuCommand { UpdateModel=model});
        if(!result){
            return BadRequest("Update Fail!");
        }
        return NoContent();
    }

    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id){
        var result=  await _mediator.Send(new DeleteProductMenuCommand { Id=id});
        if(!result){
            return BadRequest("Delete Fail!");
        }
        return NoContent();
    }
    #endregion


}