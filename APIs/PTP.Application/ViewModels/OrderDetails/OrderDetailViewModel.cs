namespace PTP.Application.ViewModels.OrderDetails;

public class OrderDetailViewModel
{
    public Guid Id { get; set; }
    public decimal ActualPrice { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
    public Guid ProductMenuId { get; set; }
    public Guid OrderId { get; set; } = default!;
    public Guid MenuId { get; set; } = default!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public decimal ProductPrice { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageURL { get; set; } = default!;
}