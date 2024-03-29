namespace PTP.Application.ViewModels.Stores;

public class StoreReportModel
{
    public List<decimal>? SaleValuesNew { get; set; }
    public List<decimal>? SaleValuesLast { get; set; }
    public List<int>? TotalOrderNew { get; set; }
    public List<DateValue>? TotalOrderLast { get; set; }
    public List<ProductMost>? ProductMosts { get; set; }
    public List<CustomerMost>? CustomerMosts { get; set; }

}

public class DateValue
{
    public string? DayOfWeek { get; set; }
    public int Value { get; set; }
}

public class ProductMost
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; } = default!;
    public string ImageURL { get; set; } = default!;
    public int TotalQuantity { get; set; }
    public decimal Price { get; set; }
}

public class CustomerMost
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = default!;
    public int TotalOrder { get; set; }
    public decimal TotalMoney { get; set; }

}