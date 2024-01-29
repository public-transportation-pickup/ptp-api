namespace PTP.Application.ViewModels.OrderDetails;

public class OrderDetailCreateModel
{
    public decimal ActualPrice { get; set; } = 0;
    public int Quantity { get; set; } = 1;
    public string? Note { get; set; }
    public Guid ProductMenuId { get; set; }
}