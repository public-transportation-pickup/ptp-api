namespace PTP.Application.ViewModels.Reports;
public class AdminReportViewModel
{
    public int Routes { get; set; }
    public int Stations { get; set; }
    public int Stores { get; set; }
    public List<TopOrderStoreModel> TopOrderStores { get; set; } = new();
    public List<TopProfitStoreModel> TopProfitStores { get; set; } = new();
    public List<TopOrderStationModel> TopOrderStations { get; set; } = new();
}

public class TopProfitStoreModel
{
    public string Name { get; set; } = string.Empty;
    public decimal TotalValue { get; set; }
}

public class TopOrderStoreModel
{
    public string Name { get; set; } = string.Empty;
    public int OrderCanceled { get; set; }
    public int OrderCompleted { get; set; }
    public int OrderOthers { get; set; }
}

public class TopOrderStationModel
{
    public string Name { get; set; } = string.Empty;
    public int OrderCanceled { get; set; }
    public int OrderCompleted { get; set; }
    public int OrderOthers { get; set; }
}