using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class StoreStation : BaseEntity
{
    [Precision(18, 2)]
    public decimal Distance { get; set; } = 0;
    public DateTime RegistrationDate { get; set; } = default!;

    #region Relationship Configuration 
    public Guid StoreId { get; set; } = default!;
    public Store Store { get; set; } = default!;
    public Guid StationId { get; set; } = default!;
    public Station Station {get; set;} = default!;
    #endregion

}