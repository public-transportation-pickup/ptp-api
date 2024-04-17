
namespace PTP.Application.IntergrationServices.Models.VNPay;
public class VNPayRefundRequestModel
{
    public string vnp_RequestId { get; set; } = string.Empty;
    public string vnp_Version { get; set; } = string.Empty;
    public string vnp_Command { get; set; } = "refund";
    public string vnp_TmnCode { get; set; } = string.Empty;
    public string vnp_TransactionType { get; set; } = string.Empty;
    public string vnp_TxnRef { get; set; } = string.Empty;
    public string vnp_Amount { get; set; } = string.Empty;
    public string vnp_OrderInfo { get; set; } = string.Empty;
    public string vnp_TransactionNo { get; set; } = string.Empty;
    public string vnp_TransactionDate { get; set; } = string.Empty;
    public string vnp_CreateBy { get; set; } = string.Empty;
    public string vnp_CreateDate { get; set; } = string.Empty;
    public string vnp_IpAddr { get; set; } = string.Empty;
    public string vnp_SecureHash { get; set; } = string.Empty;

   
}
