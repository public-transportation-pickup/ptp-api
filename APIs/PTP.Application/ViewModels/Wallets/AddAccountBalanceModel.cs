using PTP.Domain.Enums;

namespace PTP.Application.ViewModels.Wallets;
public class AddAccountBalanceModel
{
    public decimal Amount { get; set; } = 0;
    public string TxnRef { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Type { get; set; } = WalletLogTypeEnum.Deposit.ToString();

}