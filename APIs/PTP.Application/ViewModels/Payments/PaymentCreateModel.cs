using PTP.Domain.Enums;

namespace PTP.Application.ViewModels.Payments;

public class PaymentCreateModel
{
    public decimal Total { get; set; } = default!;
    public string PaymentType { get; set; } = default!;
}