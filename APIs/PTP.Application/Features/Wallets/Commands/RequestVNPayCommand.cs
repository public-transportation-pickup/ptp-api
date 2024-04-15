using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PTP.Application.IntergrationServices.Models;
using PTP.Application.Services.Interfaces;

namespace PTP.Application.Features.Wallets.Commands;
public class RequestVNPayCommand : IRequest<string>
{
    public decimal Amount { get; set; }
    public class CommandHandler : IRequestHandler<RequestVNPayCommand, string>
    {
        private readonly IClaimsService claimsService;
        private readonly AppSettings appSettings;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;
        public CommandHandler(IUnitOfWork unitOfWork,
            IClaimsService claimsService,
            AppSettings appSettings,
            IWebHostEnvironment env)
        {
            this.env = env;
            this.appSettings = appSettings;
            this.claimsService = claimsService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(RequestVNPayCommand request, CancellationToken cancellationToken)
        {
            Guid userId = claimsService.GetCurrentUser;
            var currentUser = await unitOfWork.UserRepository.GetByIdAsync(userId, x => x.Wallet);


            if (currentUser?.Wallet is null)
            {
                throw new Exception($"Error {nameof(AddAccountBalanceCommand)}-Wallet is null");
            }
            var tick = DateTime.Now.Ticks.ToString();
            PaymentRequestModel payRequest = new();

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", payRequest.Version);
            vnpay.AddRequestData("vnp_Command", payRequest.Command);
            vnpay.AddRequestData("vnp_TmnCode", appSettings.VnPay.Vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((int)request.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", payRequest.CreateDate);
            vnpay.AddRequestData("vnp_CurrCode", payRequest.CurrCode);
            vnpay.AddRequestData("vnp_IpAddr", payRequest.IpAddress);
            vnpay.AddRequestData("vnp_Locale", payRequest.Locale);

            vnpay.AddRequestData("vnp_OrderInfo", "Nạp tiền vào ví");
            vnpay.AddRequestData("vnp_OrderType", payRequest.OrderType); //default value: other
            if (env.IsDevelopment())
                vnpay.AddRequestData("vnp_ReturnUrl", $"https://localhost:7248/api/wallets/vn-pay/response?userId={userId}");
            else
                vnpay.AddRequestData("vnp_ReturnUrl", $"http://ptp-srv.ddns.net:5000/api/wallets/vn-pay/response?userId={userId}");

            vnpay.AddRequestData("vnp_TxnRef", payRequest.TxnRef); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            var paymentUrl = vnpay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
                 appSettings.VnPay.Vnp_HashSecret);
            return paymentUrl;
        }
    }
}