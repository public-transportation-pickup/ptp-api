using PTP.Application.ViewModels.OrderDetails;

namespace PTP.Application.ViewModels.Orders;
public class OrderCreateModel
{
    public string Name { get; set; } = default!;
    public string PhoneNumber {  get; set; }=default!;
    public decimal Total { get; set; } = 0;
    public Guid UserId { get; set; }
    public Guid StationId { get; set; } = default!;
    public Guid StoreId { get; set; } 

    public List<OrderDetailCreateModel> OrderDetails{get;set;}=default!;
}