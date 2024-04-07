using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PTP.Application.Features.Notifications.Commands;
using PTP.Application.Features.Notifications.Queries;
using PTP.Application.ViewModels.MongoDbs.Notifications;

namespace PTP.WebAPI.Controllers;
public class NotificationsController : BaseController
{
    private readonly IMediator mediator;
    public NotificationsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Lấy notification theo Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetById([FromRoute] ObjectId id)
        => Ok(await mediator.Send(new GetNotificationByIdQuery { Id = id }));

    /// <summary>
    /// Lấy Notifications theo current user
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet, Route("/api/users/notifications")]
    public async Task<IActionResult> GetByUser()
        => Ok(await mediator.Send(new GetNotificationByUserQuery()));

    /// <summary>
    /// Tạo mới một hoặc nhiều noti
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] List<NotificationCreateModel> models)
        => Ok(await mediator.Send(new CreateManyNotificationCommand { Models = models }));

    /// <summary>
    /// Delete Một noti
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] ObjectId id)
    {
        await mediator.Send(new DeleteNotificationCommand { Id = id });
        return NoContent();
    }


    /// <summary>
    /// Update một noti
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Update([FromBody] NotificationUpdateModel model)
    {
        await mediator.Send(new UpdateNotificationCommand { Model = model });
        return NoContent();
    }


    /// <summary>
    /// Xoá hết noti theo current user login
    /// </summary>
    /// <returns></returns>
    [HttpDelete, Route("/api/users/notifications")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteAll()
    {
        await mediator.Send(new DeleteNotificationByUserCommand());
        return NoContent();
    }
}