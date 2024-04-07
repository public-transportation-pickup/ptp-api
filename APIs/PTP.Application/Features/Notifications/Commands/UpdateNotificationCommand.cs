
using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.MongoDbs.Notifications;
using PTP.Domain.Entities.MongoDbs;

namespace PTP.Application.Features.Notifications.Commands;
public class UpdateNotificationCommand : IRequest<bool>
{
    public NotificationUpdateModel Model = new();
    public class CommandHandler : IRequestHandler<UpdateNotificationCommand, bool>
    {
        private readonly ILogger<UpdateNotificationCommand> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IClaimsService claimsService;
        private readonly INotificationRepository notificationRepository;
        public CommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService,
            INotificationRepository notificationRepository,
            ILogger<UpdateNotificationCommand> logger)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.claimsService = claimsService;
            this.notificationRepository = notificationRepository;
        }
        public async Task<bool> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            Guid userId = claimsService.GetCurrentUser;
            const string toolService = nameof(UpdateNotificationCommand);
            logger.LogInformation($"{toolService}, userId", userId);
            var entity = unitOfWork.Mapper.Map<NotificationEntity>(request.Model);
            entity.UserId = userId;
            var result = await notificationRepository.UpdateAsync(entity: entity,
                cancellationToken: cancellationToken);
            logger.LogInformation($"{toolService}_Result", result);
            return result is not null;
        }
    }
}