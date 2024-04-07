using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.MongoDbs.Notifications;
using PTP.Domain.Entities.MongoDbs;

namespace PTP.Application.Features.Notifications.Commands;
public class CreateManyNotificationCommand : IRequest<List<NotificationViewModel>?>
{
    public List<NotificationCreateModel> Models = new();
    public class CommandHandler : IRequestHandler<CreateManyNotificationCommand, List<NotificationViewModel>?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IClaimsService claimsService;
        private readonly INotificationRepository notificationRepository;
        private readonly ILogger<CreateManyNotificationCommand> logger;
        public CommandHandler(IUnitOfWork unitOfWork,
            IClaimsService claimsService,
            INotificationRepository notificationRepository,
            ILogger<CreateManyNotificationCommand> logger)
        {
            this.unitOfWork = unitOfWork;
            this.notificationRepository = notificationRepository;
            this.logger = logger;
            this.claimsService = claimsService;
        }
        public async Task<List<NotificationViewModel>?> Handle(CreateManyNotificationCommand request, CancellationToken cancellationToken)
        {
            const string toolService = nameof(CreateManyNotificationCommand);
            Guid userId = claimsService.GetCurrentUser;
            logger.LogInformation($"{toolService}_userId", userId);
            logger.LogInformation($"{toolService}_input", request.Models);
            var entities = unitOfWork.Mapper.Map<List<NotificationEntity>>(request.Models);
            entities.ForEach(x => x.UserId = userId);
            var result = await notificationRepository.CreateManyAsync(entities: entities,
                cancellationToken: cancellationToken);
            logger.LogInformation($"{toolService}_result", result);
            return unitOfWork.Mapper.Map<List<NotificationViewModel>>(result);

        }
    }
}