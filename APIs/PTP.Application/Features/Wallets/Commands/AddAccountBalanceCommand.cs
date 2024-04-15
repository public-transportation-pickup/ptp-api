using MediatR;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Wallets;
using PTP.Domain.Entities;
using PTP.Domain.Enums;

namespace PTP.Application.Features.Wallets.Commands;
public class AddAccountBalanceCommand : AddAccountBalanceModel, IRequest<bool>
{
    public class CommandHandler : IRequestHandler<AddAccountBalanceCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IClaimsService claimsService;
        public CommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            this.unitOfWork = unitOfWork;
            this.claimsService = claimsService;

        }
        public async Task<bool> Handle(AddAccountBalanceCommand request, CancellationToken cancellationToken)
        {

            if (claimsService.GetCurrentUser == Guid.Empty)
            {
                throw new Exception($"Error: {nameof(AddAccountBalanceCommand)}-no_current_user");
            }
            var currentUser = await unitOfWork.UserRepository.GetByIdAsync(claimsService.GetCurrentUser, x => x.Wallet);


            if (currentUser?.Wallet is null)
            {
                throw new Exception($"Error {nameof(AddAccountBalanceCommand)}-Wallet is null");
            }
            else
            {
                // Add Money
                currentUser.Wallet.Amount += request.Amount;

                var walletLog = new
                WalletLog
                {
                    Amount = request.Amount,
                    Source = request.Source,
                    TxnRef = request.TxnRef,
                    Type = nameof(WalletLogTypeEnum.Deposit),
                    WalletId = currentUser.Wallet.Id
                };
                unitOfWork.WalletRepository.Update(currentUser.Wallet);
                await unitOfWork.WalletLogRepository.AddAsync(walletLog);
                return await unitOfWork.SaveChangesAsync();
            }

        }
    }
}