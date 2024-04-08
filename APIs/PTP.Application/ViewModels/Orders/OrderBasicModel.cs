using PTP.Application.ViewModels.OrderDetails;

namespace PTP.Application.ViewModels.Orders;

public class OrderBasicModel
{
    public Guid StoreId { get; set; }
    public int OrderWaiting { get; set; }
    public int OrderPreparing { get; set; }
    public int OrderPrepared { get; set; }
    public int OrderCompleted { get; set; }
    public int OrderCanceled { get; set; }
    public int Total { get; set; }

}