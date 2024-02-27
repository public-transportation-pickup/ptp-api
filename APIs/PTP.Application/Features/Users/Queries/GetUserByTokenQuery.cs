using Hangfire.Logging;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Users;

namespace PTP.Application.Features.Users.Queries;
public class GetUserByTokenQuery : IRequest<UserViewModel>
{
    public class QueryHandler : IRequestHandler<GetUserByTokenQuery, UserViewModel>
    {
        private readonly IClaimsService claimsService;
        private readonly IUnitOfWork unitOfWork;
        private ILogger<GetUserByTokenQuery> logger;
        public QueryHandler(IUnitOfWork unitOfWork, IClaimsService claimsService, ILogger<GetUserByTokenQuery> logger)
        {
            this.claimsService = claimsService;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }
        public async Task<UserViewModel> Handle(GetUserByTokenQuery request, CancellationToken cancellationToken)
        {
            const string toolService = nameof(GetUserByTokenQuery);
            var userId = claimsService.GetCurrentUser;
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId) 
                ?? throw new Exception($"Error at {nameof(toolService)}_User not found");
            return unitOfWork.Mapper.Map<UserViewModel>(user);
        }
    }
}