namespace PTP.Application.ViewModels.Stores;

public class StoreReportModel
{
    public Guid StoreId { get; set; }
    public string StoreName { get; set; }=default!;
    public string StoreAddress { get; set; }=default!;
    public decimal WalletAmount { get; set; }
    public decimal TotalSalesNew { get; set; }
    public decimal TotalSalesLast { get; set; }

    public int TotalOrdersNew { get; set; }
    public int TotalOrdersLast { get; set; }
    public decimal AverageSaleValueNew { get; set; }
    public decimal AverageSaleValueLast { get; set; }
    public int VisitorsNew { get; set; }
    public int VisitorsLast { get; set; }
    public List<decimal>? SaleValuesNew { get; set; }
    public List<decimal>? SaleValuesLast { get; set; }
    public List<int>? TotalOrderNew { get; set; }
    public List<int>? TotalOrderLast { get; set; }
    // public List<DateValue>? TotalOrderNew { get; set; }
    // public List<DateValue>? TotalOrderLast { get; set; }
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