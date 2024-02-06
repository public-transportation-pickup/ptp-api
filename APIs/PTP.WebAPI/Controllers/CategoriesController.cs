using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Categories.Commands;
using PTP.Application.Features.Categories.Queries;
using PTP.Application.Features.Menus.Commands;
using PTP.Application.Features.Menus.Queries;
using PTP.Application.Features.Products.Queries;
using PTP.Application.ViewModels.Categories;
using PTP.Application.ViewModels.Menus;

namespace PTP.WebAPI.Controllers;

public class CategoriesController:BaseController
{
    public readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator=mediator;
    }

    #region Queries
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int pageNumber = 0,
                                        [FromQuery] int pageSize = 10, 
                                        [FromQuery] Dictionary<string, string> filter = default!) 
    => Ok(await _mediator.Send(new GetAllCategoryQuery{PageNumber=pageNumber,PageSize=pageSize,Filter=filter}));



    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    => Ok(await _mediator.Send(new GetCategoryByIdQuery { Id = id }));


    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id}/products")]
    public async Task<IActionResult> GetProductsByCategoryId([FromRoute] Guid id,
                                                            [FromQuery] int pageNumber = 0,
                                                            [FromQuery] int pageSize = 10, 
                                                            [FromQuery] Dictionary<string, string> filter = default!)
    => Ok(await _mediator.Send(new GetProductsByCategoryIdQuery { CategoryId = id ,PageNumber=pageNumber,PageSize=pageSize,Filter=filter}));
    #endregion 

    #region Commands
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateModel model){
        var result=  await _mediator.Send(new CreateCategoryCommand { CreateModel=model });
        if(result is null){
            return BadRequest("Create Fail!");
        }
        return CreatedAtAction(nameof(GetById),new {Id=result.Id},result);
    }
 


    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,CategoryUpdateModel model){
        if(id!=model.Id) return BadRequest("Id is not match!");
        var result=  await _mediator.Send(new UpdateCategoryCommand { UpdateModel=model});
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
        var result=  await _mediator.Send(new DeleteCategoryCommand { Id=id});
        if(!result){
            return BadRequest("Delete Fail!");
        }
        return NoContent();
    }
    #endregion
}