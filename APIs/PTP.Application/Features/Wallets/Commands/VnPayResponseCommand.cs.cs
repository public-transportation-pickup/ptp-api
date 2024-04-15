using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.IntergrationServices.Models;
using PTP.Domain.Entities;
using PTP.Domain.Enums;

namespace PTP.Application.Features.Wallets.Commands
{
	public class VnPayResponseCommand : IRequest<bool>
	{
		public VnPayResponseModel Model { get; set; } = new();
		public class CommandHandler : IRequestHandler<VnPayResponseCommand, bool>
		{
			private readonly IUnitOfWork unitOfWork;
			private readonly ILogger<VnPayResponseCommand> logger;
			public CommandHandler(IUnitOfWork unitOfWork,
					ILogger<VnPayResponseCommand> logger)
			{
				this.logger = logger;
				this.unitOfWork = unitOfWork;
			}
			public async Task<bool> Handle(VnPayResponseCommand request, CancellationToken cancellationToken)
			{

				const string toolService = nameof(VnPayResponseCommand);
				var input = new { request.Model };
				if (request.Model.vnp_TransactionStatus != "00")
				{
					return false;
				}
				VnPayLibrary vnPay = new();
				logger.LogInformation($"Source {toolService} _ input: {input}");
				var wallet = await unitOfWork.WalletRepository.FirstOrDefaultAsync(x => x.UserId == input.Model.userId);
				if (wallet is null)
					throw new ArgumentException($"Source: {toolService}_Wallet is null");

				var isExisted = await unitOfWork.WalletLogRepository.FirstOrDefaultAsync(x => x.TransactionNo == request.Model.vnp_TransactionNo
					&& x.TxnRef == request.Model.vnp_TxnRef);
				if (isExisted is not null)
				{
					return false;
				}

				var walletLog = new WalletLog()
				{
					Amount = request.Model.vnp_Amount / 100,
					//CreationDate = DateTime.Parse(request.Model.vnp_PayDate),
					TxnRef = request.Model.vnp_TxnRef,
					TransactionNo = request.Model.vnp_TransactionNo,
					Source = "VNPay",
					Type = nameof(WalletLogTypeEnum.Deposit),
					WalletId = wallet.Id,
					CreationDate = DateTime.Now

				};
				wallet.Amount += walletLog.Amount;
				unitOfWork.WalletRepository.Update(wallet);
				await unitOfWork.WalletLogRepository.AddAsync(walletLog);

				return await unitOfWork.SaveChangesAsync();


			}
		}

	}
}
