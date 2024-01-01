using PTP.Application.Repositories.Interfaces;

namespace PTP.Application;
public interface IUnitOfWork
{
    IRoleRepository RoleRepository { get; }
    Task<bool> SaveChangesAsync();
}