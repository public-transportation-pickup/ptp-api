using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Repositories.Interfaces.MongoDbs;
using PTP.Application.Services.Interfaces;

namespace PTP.Application.Features.Notifications.Commands;
public class DeleteNotificationByUserCommand : IRequest<bool>
{

    public class CommandHandler : IRequestHandler<DeleteNotificationByUserCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IClaimsService claimsService;
        private readonly INotificationRepository notificationRepository;
        private readonly ILogger<DeleteNotificationByUserCommand> logger;
        public CommandHandler(IUnitOfWork unitOfWork,
            ILogger<DeleteNotificationByUserCommand> logger,
            INotificationRepository notificationRepository,
            IClaimsService claimsService)
        {
            this.claimsService = claimsService;
            this.notificationRepository = notificationRepository;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteNotificationByUserCommand request, CancellationToken cancellationToken)
        {
            Guid userId = claimsService.GetCurrentUser;
            const string toolService = nameof(DeleteNotificationByUserCommand);
            logger.LogInformation($"{toolService}, currentUser", userId);
            var result = await notificationRepository.DeleteAllAsync(userId: userId,
                cancellationToken: cancellationToken);
            return result;
        }
    }
}