namespace PTP.Application.ViewModels.OrderDetails;

public class OrderDetailViewModel
{
    public Guid Id{get;set;}
    public decimal ActualPrice { get; set; } 
    public int Quantity { get; set; } 
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageURL { get; set; } = default!;
}