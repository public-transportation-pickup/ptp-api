using System.ComponentModel;

namespace PTP.Domain.Entities;
public class MenuCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    #region Relationship Configuration
    public Guid MenuId { get; set; }
    public Menu Menu { get; set; } = default!;
    public Guid CategoryId { get; set; } = default!;
    public Category Category { get; set; } = default!;
    #endregion
}