using PTP.Application;
using PTP.Application.Repositories.Interfaces;

namespace PTP.Infrastructure;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public UnitOfWork(AppDbContext dbcontext, IRoleRepository roleRepository)
    {
        _dbContext = dbcontext;
        RoleRepository = roleRepository;
    }
    public IRoleRepository RoleRepository { get; }

    public async Task<bool> SaveChangesAsync() => (await _dbContext.SaveChangesAsync()) > 0;
    
    
    
}