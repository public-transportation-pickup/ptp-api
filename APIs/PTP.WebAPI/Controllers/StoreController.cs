using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTP.Application.Features.Routes.Queries;
using PTP.Application.Features.Stores.Commands;
using PTP.Application.Features.Stores.Queries;
using PTP.Application.ViewModels.Stores;
using System.Net;

namespace PTP.WebAPI.Controllers
{
    public class StoreController:BaseController
    {
        private readonly IMediator _mediator;

        public StoreController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #region QUERIES
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _mediator.Send(new GetAllStoreQuery()));



        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetStoreByIdQuery { Id = id }));
        #endregion

        #region COMMANDS

        
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]StoreCreateModel model){
            var result=  await _mediator.Send(new CreateStoreCommand { CreateModel=model });
            if(result is null){
                return BadRequest("Create Fail!");
            }
            return CreatedAtAction(nameof(GetById),new {Id=result.Id},result);
        }
       


        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,[FromForm]StoreUpdateModel model){
            if(id!=model.Id) return BadRequest("Id is not match!");
            var result=  await _mediator.Send(new UpdateStoreCommand { StoreUpdate=model});
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
            var result=  await _mediator.Send(new DeleteStoreCommand { Id=id});
            if(!result){
                return BadRequest("Delete Fail!");
            }
            return NoContent();
        }


        #endregion

    }
}
