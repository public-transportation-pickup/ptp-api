namespace PTP.Domain.Entities;
public class StoreType : BaseEntity
{
    public string Name { get; set; } = default!;
    public ICollection<Store> Stores { get; set; } = default!;
}