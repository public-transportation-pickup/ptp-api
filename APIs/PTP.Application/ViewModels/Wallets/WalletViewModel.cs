using PTP.Application.ViewModels.Transactions;
using PTP.Application.ViewModels.WalletLogs;

namespace PTP.Application.ViewModels.Wallets;
public class WalletViewModel
{
    public Guid Id{get;set;}
    public string Name { get; set; } = default!;
    public decimal Amount { get; set; }
    public string? WalletType { get; set; } 
    public Guid? UserId{get;set;}
    public Guid? StoreId{get;set;}

    public IEnumerable<TransactionViewModel>? Transactions { get; set; }
    public IEnumerable<WalletLogViewModel>? WalletLogs {  get; set; }
}