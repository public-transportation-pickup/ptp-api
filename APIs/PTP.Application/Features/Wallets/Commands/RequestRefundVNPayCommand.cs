// using MediatR;
// using Microsoft.Extensions.Logging;
// using PTP.Application.Services.Interfaces;

// namespace PTP.Application.Features.Wallets.Commands;
// public class RequestRefundVNPayCommand : IRequest<string>
// {
//     public class CommandHandler : IRequestHandler<RequestRefundVNPayCommand, string>
//     {
//         private readonly IUnitOfWork unitOfWork;
//         private readonly IClaimsService claimsService;
//         private readonly ILogger<RequestRefundVNPayCommand> logger;

//         public CommandHandler(IUnitOfWork unitOfWork,
//             IClaimsService claimsService,
//             ILogger<RequestRefundVNPayCommand> logger)
//         {
//             this.unitOfWork = unitOfWork;
//             this.logger = logger;
//             this.claimsService = claimsService;
//         }
//         public Task<string> Handle(RequestRefundVNPayCommand request, CancellationToken cancellationToken)
//         {
//             const string toolService = nameof(RequestRefundVNPayCommand);
//             throw new NotImplementedException();
//         }

//     }
// }