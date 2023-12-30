using System.ComponentModel.DataAnnotations;

namespace PTP.Domain.Entities;
public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; } = Guid.Empty;
    public DateTime? ModificationDate { get; set; } = null;
    public Guid? ModificatedBy { get; set; } = default!;
    public bool IsDeleted { get; set; } = false;
}