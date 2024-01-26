using PTP.Application.ViewModels.OrderDetails;
using PTP.Application.ViewModels.Payments;

namespace PTP.Application.ViewModels.Orders;
public class OrderCreateModel
{
    public string Name { get; set; } = default!;
    public string PhoneNumber {  get; set; }=default!;
    public string PickUpTime { get; set; } = default!;
    public decimal Total { get; set; } = 0;
    public Guid UserId { get; set; }
    public Guid StationId { get; set; } = default!;
    public Guid StoreId { get; set; } 
    public PaymentCreateModel Payment{get;set;}=default!;
    public List<OrderDetailCreateModel> OrderDetails{get;set;}=default!;
}