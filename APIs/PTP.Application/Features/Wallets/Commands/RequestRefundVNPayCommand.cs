using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.IntergrationServices.Models;
using PTP.Application.Services.Interfaces;

namespace PTP.Application.Features.Wallets.Commands;
public class RequestRefundVNPayCommand : IRequest<string>
{
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
            var userId = claimsService.GetCurrentUser;
            logger.LogInformation($"Source: {toolService}_UserId: {userId}");
            var tick = DateTime.Now.Ticks.ToString();
            PaymentRequestModel payRequest = new() 
            {
                Command = "refund",

            };

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", payRequest.Version);
            vnpay.AddRequestData("vnp_Command", payRequest.Command);
            vnpay.AddRequestData("vnp_TmnCode", appSettings.VnPay.Vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", "2000000");
            vnpay.AddRequestData("vnp_TranSactionType", "02");
            vnpay.AddRequestData("vnp_CreateDate", payRequest.CreateDate);
            vnpay.AddRequestData("vnp_CurrCode", payRequest.CurrCode);
            vnpay.AddRequestData("vnp_IpAddr", payRequest.IpAddress);
            vnpay.AddRequestData("vnp_Locale", payRequest.Locale);

            vnpay.AddRequestData("vnp_OrderInfo", "Nạp tiền vào ví");
            vnpay.AddRequestData("vnp_OrderType", payRequest.OrderType); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", $"http://ptp-srv.ddns:8001/?userId={userId}");

            vnpay.AddRequestData("vnp_TxnRef", payRequest.TxnRef); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            var paymentUrl = vnpay.CreateRequestUrl("https://sandbox.vnpayment.vn/merchant_webapi/api/transaction",
                 appSettings.VnPay.Vnp_HashSecret);
            return paymentUrl;
        }

    }
}