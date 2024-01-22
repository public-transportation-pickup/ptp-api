using PTP.Application.ViewModels.OrderDetails;

namespace PTP.Application.ViewModels.Orders;

public class OrderViewModel
{
    public Guid Id{get;set;}
    public string Name { get; set; } = default!;
    public string PhoneNumber {  get; set; }=default!;
    public decimal Total { get; set; } = 0;
    public string Status { get; set; } = default!;
    public Guid UserId { get; set; }
    public Guid StationId { get; set; } = default!;
    public string StationName{get;set;}=default!;
    public Guid StoreId { get; set; }
    public string StoreName{get;set;}=default!;
    public string StorePhoneNumber{get;set;}=default!;
    public Guid PaymentId{get;set;}
    public string PaymentType { get; set; } = default!;
    public string? PaymentStatus { get; set; }
    public IEnumerable<OrderDetailViewModel>? OrderDetails{get;set;}

}