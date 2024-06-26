using PTP.Application.ViewModels.OrderDetails;

namespace PTP.Application.ViewModels.Orders;

public class OrderViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public DateTime PickUpTime { get; set; } = default!;
    public int TotalPreparationTime { get; set; }
    public string? CanceledReason { get; set; }
    public decimal Total { get; set; } = 0;
    public string Status { get; set; } = default!;
    public Guid UserId { get; set; }
    public Guid StationId { get; set; } = default!;
    public string StationName { get; set; } = default!;
    public string StationAddress { get; set; } = default!;
    public Guid StoreId { get; set; }
    public string StoreName { get; set; } = default!;
    public string StoreAddress { get; set; } = string.Empty;
    public string StorePhoneNumber { get; set; } = default!;
    public Guid PaymentId { get; set; }
    public string PaymentType { get; set; } = default!;
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; } = null;

    public string? PaymentStatus { get; set; }
    // public Guid MenuId { get; set; }
    public decimal? ReturnAmount { get; set; } = null;
    public IEnumerable<OrderDetailViewModel>? OrderDetails { get; set; }

}