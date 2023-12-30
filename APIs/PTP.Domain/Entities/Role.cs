using System.ComponentModel.DataAnnotations;
using PTP.Domain.Enums;

namespace PTP.Domain.Entities;
public class Role : BaseEntity
{
    public string Name { get; set; } = RoleEnum.Customer.ToString().ToUpper();
    public ICollection<User> Users { get; set; } = default!;


}