using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using PTP.Application.Commons;
using PTP.Application.Repositories.Interfaces;
using PTP.Application.Services.Interfaces;
using PTP.Domain.Entities;

namespace PTP.Infrastructure.Repositories;
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{

    protected DbSet<TEntity> _dbSet;
    private readonly ICurrentTime _timeService;
    private readonly IClaimsService _claimsService;

    public GenericRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService)
    {
        _dbSet = context.Set<TEntity>();
        _timeService = timeService;
        _claimsService = claimsService;
    }
    public async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes) =>
        await includes
       .Aggregate(_dbSet.AsQueryable(),
           (entity, property) => entity.Include(property).IgnoreAutoIncludes())
       .Where(x => x.IsDeleted == false)
        .OrderByDescending(x => x.CreationDate)
       .ToListAsync();

    public async Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes)
    {
        return await includes
           .Aggregate(_dbSet.AsQueryable(),
               (entity, property) => entity.Include(property))
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.Id.Equals(id) && x.IsDeleted == false);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreationDate = _timeService.GetCurrentTime();
        entity.CreatedBy = _claimsService.GetCurrentUser;
        var result = await _dbSet.AddAsync(entity);
        return result.Entity;
    }

    public void SoftRemove(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.ModificatedBy = _claimsService.GetCurrentUser;
        entity.ModificationDate = _timeService.GetCurrentTime();
        _dbSet.Update(entity);
    }

    public void Update(TEntity entity)
    {
        entity.ModificationDate = _timeService.GetCurrentTime();
        entity.ModificatedBy = _claimsService.GetCurrentUser;
        _dbSet.Update(entity);
    }

    public async Task AddRangeAsync(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreationDate = _timeService.GetCurrentTime();
            entity.CreatedBy = _claimsService.GetCurrentUser;
        }
        await _dbSet.AddRangeAsync(entities);
    }

    public void SoftRemoveRange(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.ModificationDate = _timeService.GetCurrentTime();
            entity.ModificatedBy = _claimsService.GetCurrentUser;
        }
        _dbSet.UpdateRange(entities);
    }

    public async Task<Pagination<TEntity>> ToPagination(int pageIndex = 0, int pageSize = 10)
    {
        var itemCount = await _dbSet.CountAsync();
        var items = await _dbSet.Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .AsNoTracking()
                                .ToListAsync();

        var result = new Pagination<TEntity>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = items,
        };

        return result;
    }

    public void UpdateRange(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreationDate = _timeService.GetCurrentTime();
            entity.CreatedBy = _claimsService.GetCurrentUser;
        }
        _dbSet.UpdateRange(entities);
    }

    public async Task<TEntity> FindByField(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
    => await includes
       .Aggregate(_dbSet!.AsQueryable(),
           (entity, property) => entity!.Include(property)).AsNoTracking()
       .Where(expression!)
        .FirstAsync(x => x.IsDeleted == false);

    public async Task<List<TEntity>> FindListByField(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
    => await includes
       .Aggregate(_dbSet!.AsQueryable(),
           (entity, property) => entity.Include(property)).AsNoTracking()
       .Where(expression!)
       .Where(x => x.IsDeleted == false)
        .OrderByDescending(x => x.CreationDate)
        .ToListAsync();

    public async Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
      => await includes
     .Aggregate(_dbSet!.AsQueryable(),
         (entity, property) => entity.Include(property)).AsNoTracking()
     .Where(expression!)
     .Where(x => x.IsDeleted == false)
      .OrderByDescending(x => x.CreationDate)
      .ToListAsync();

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
     => await includes
    .Aggregate(_dbSet!.AsQueryable(),
        (entity, property) => entity!.Include(property)).AsNoTracking()
    .Where(expression!)
     .FirstOrDefaultAsync(x => x.IsDeleted == false);
}
