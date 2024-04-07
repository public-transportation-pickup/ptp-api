using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Application.ViewModels.MongoDbs.Notifications;

namespace PTP.Application.Features.Notifications.Queries;
public class GetNotificationByIdQuery : IRequest<NotificationViewModel?>
{
    // Todo
    public ObjectId Id;
    public class QueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationViewModel?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationRepository notificationRepository;
        private ILogger<GetNotificationByIdQuery> logger;
        public QueryHandler(IUnitOfWork unitOfWork,
            ILogger<GetNotificationByIdQuery> logger,
            INotificationRepository notificationRepository)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.notificationRepository = notificationRepository;
        }
        public async Task<NotificationViewModel?> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            const string toolService = nameof(GetNotificationByIdQuery);
            logger.LogInformation($"{toolService}_input", request.Id);
            var result = (await notificationRepository.GetAll(cancellationToken: cancellationToken))?.FirstOrDefault(x => x.Id == request.Id);
            logger.LogInformation($"{toolService}_result", result);
            return unitOfWork.Mapper.Map<NotificationViewModel>(result);
        }
    }
}