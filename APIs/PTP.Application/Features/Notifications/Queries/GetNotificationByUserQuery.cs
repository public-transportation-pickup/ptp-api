using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.MongoDbs.Notifications;

namespace PTP.Application.Features.Notifications.Queries;
public class GetNotificationByUserQuery : IRequest<List<NotificationViewModel>?>
{
    public class QueryHandler : IRequestHandler<GetNotificationByUserQuery, List<NotificationViewModel>?>
    {
        private readonly ILogger<GetNotificationByUserQuery> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationRepository notificationRepository;
        private readonly IClaimsService claimsService;
        public QueryHandler(ILogger<GetNotificationByUserQuery> logger,
            IUnitOfWork unitOfWork,
            INotificationRepository notificationRepository,
            IClaimsService claimsService)
        {
            this.logger = logger;
            this.claimsService = claimsService;
            this.unitOfWork = unitOfWork;
            this.notificationRepository = notificationRepository;
        }
        public async Task<List<NotificationViewModel>?> Handle(GetNotificationByUserQuery request, CancellationToken cancellationToken)
        {
            Guid userId = claimsService.GetCurrentUser;
            const string toolService = nameof(GetNotificationByUserQuery);
            logger.Log(LogLevel.Information, $"Source: {toolService}, userId", userId);
            var result = await notificationRepository.GetByUser(userId: userId,
                cancellationToken: cancellationToken);
            logger.Log(LogLevel.Information, $"Source: {toolService}, Result", result?.Count);
            return unitOfWork.Mapper.Map<List<NotificationViewModel>?>(result);
        }
    }
}