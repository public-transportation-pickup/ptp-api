using Microsoft.EntityFrameworkCore;

namespace PTP.Domain.Entities;
public class Store : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Status { get; set; } = default!;
    public byte OpenedTime { get; set; } = default!;
    public byte ClosedTime { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Latitude { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Longitude { get; set; } = default!;
    public string Address { get; set; } = default!;
    public DateTime? ActivationDate { get; set; } = null;
    [Precision(18, 2)]
    public decimal CommisionRate { get; set; } = default!;

    #region Image 
    public string ImageName { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    #endregion

    #region  Relationship Configuration
    public ICollection<StoreStation> StoreStations { get; set; } = default!;
    public ICollection<Session> Sessions { get; set; } = default!;
    public ICollection<Product> Products { get; set; } = default!;
    public ICollection<Menu> Menus { get; set; } = default!;
    #endregion
}