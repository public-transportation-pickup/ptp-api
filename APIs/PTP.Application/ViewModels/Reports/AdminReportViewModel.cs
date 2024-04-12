namespace PTP.Application.ViewModels.Reports;
public class AdminReportViewModel
{
    public int Routes { get; set; }
    public int Stations { get; set; }
    public int Stores { get; set; }
    public double Revenue { get; set; }
    public List<decimal> SaleValueCurrent { get; set; } = new();
    public List<decimal> SaleValueLast { get; set; } = new();
    public List<TopOrderStoreModel> TopOrderStores { get; set; } = new();
    public List<TopOrderStationModel> TopOrderStations { get; set; } = new();
    public List<CategoryReportViewModel> Categories { get; set; } = new();

}

public class TopOrderStoreModel
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int OrderCanceled { get; set; }
    public int OrderCompleted { get; set; }
    public int OrderOthers { get; set; }
    public decimal Revenue { get; set; }
}

public class TopOrderStationModel
{
    public string Name { get; set; } = string.Empty;
    public int OrderCanceled { get; set; }
    public int OrderCompleted { get; set; }
    public int OrderOthers { get; set; }
}