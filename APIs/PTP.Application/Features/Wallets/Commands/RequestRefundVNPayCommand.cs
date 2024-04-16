using Microsoft.Win32.SafeHandles;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using PTP.Application.IntergrationServices.Models;
using PTP.Application.IntergrationServices.Models.VNPay;
using PTP.Application.Services.Interfaces;

namespace PTP.Application.Features.Wallets.Commands;
public class RequestRefundVNPayCommand : IRequest<string>
{
    public string TxnRef { get; set; } = string.Empty;
    public class CommandHandler : IRequestHandler<RequestRefundVNPayCommand, string>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IClaimsService claimsService;
        private readonly ILogger<RequestRefundVNPayCommand> logger;
        private readonly AppSettings appSettings;

        public CommandHandler(IUnitOfWork unitOfWork,
            IClaimsService claimsService,
            ILogger<RequestRefundVNPayCommand> logger,
            AppSettings appSettings)
        {
            this.appSettings = appSettings;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.claimsService = claimsService;
        }
        public async Task<string> Handle(RequestRefundVNPayCommand request, CancellationToken cancellationToken)
        {
            const string toolService = nameof(RequestRefundVNPayCommand);
            var input = new { request.TxnRef };
            var userId = claimsService.GetCurrentUser;
            logger.LogInformation($"Source: {toolService}_UserId: {userId}");
            logger.LogInformation($"Source: {toolService}_input: {input}");
            var tick = DateTime.Now.Ticks.ToString();
            var walletLog = await unitOfWork.WalletLogRepository.FirstOrDefaultAsync(x => x.TxnRef == request.TxnRef,
                x => x.Wallet);
            if (walletLog is null)
            {
                throw new Exception($"Source {toolService}_no_data_found at txnRef: {input}");
            }
            else if (walletLog.Wallet.UserId != userId)
            {
                throw new Exception($"Source {toolService}_Can not refund_ wallet not belong to the user request");
            }
            PaymentRequestModel payRequest = new()
            {
                Command = "querydr",

            };

            var vnpay = new VnPayLibrary();


            var paymentUrl = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";

            VNPayRefundRequestModel requestModel = new()
            {
                vnp_RequestId = DateTime.Now.Ticks.ToString(),
                vnp_Version = payRequest.Version,
                vnp_Command = payRequest.Command,
                vnp_TmnCode = appSettings.VnPay.Vnp_TmnCode,
                vnp_TransactionType = "02",
                vnp_TxnRef = payRequest.TxnRef,
                vnp_Amount = ((int)walletLog.Amount * 100).ToString(),
                vnp_OrderInfo = "Hoàn tiền",
                vnp_TransactionNo = walletLog.TransactionNo,
                vnp_TransactionDate = walletLog.CreationDate.ToString("yyyyMMddHHmmss"),
                vnp_CreateBy = walletLog.Wallet.UserId.ToString() ?? "Undefined",
                vnp_CreateDate = payRequest.CreateDate,
                vnp_IpAddr = payRequest.IpAddress,

            };
            
            var signData = requestModel.vnp_RequestId + "|" 
                + requestModel.vnp_Version + "|" 
                + requestModel.vnp_Command + "|" 
                + requestModel.vnp_TmnCode + "|" 
                + requestModel.vnp_TransactionType 
                + "|" + requestModel.vnp_TxnRef 
                + "|" + requestModel.vnp_Amount 
                + "|" + requestModel.vnp_TransactionNo 
                + "|" + requestModel.vnp_TransactionDate 
                + "|" + requestModel.vnp_CreateBy 
                + "|" + requestModel.vnp_CreateDate 
                + "|" + requestModel.vnp_IpAddr 
                + "|" + requestModel.vnp_OrderInfo;
            var secureHash = Utils.HmacSHA512(appSettings.VnPay.Vnp_HashSecret, signData);
            requestModel.vnp_SecureHash = secureHash;
            var result = await vnpay.Refund(paymentUrl, model: requestModel);
            return result;
        }

    }
}