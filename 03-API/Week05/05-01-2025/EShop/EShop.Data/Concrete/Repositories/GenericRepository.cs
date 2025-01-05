using System;
using System.Linq.Expressions;
using EShop.Data.Abstract;
using EShop.Data.Concrete.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EShop.Data.Concrete.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly EShopDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public Task AddAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public void Delete(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes)
    {
        throw new NotImplementedException();
    }

    public async Task<TEntity> GetAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet; //query dbseti temsil eden productları aldı
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => include(current));
        }

        //_dbSet=context.Products();
        //query=context.Products();
        //predicate = p => p.IsDeleted == true
        //query = context.Products.Where(p => p.IsDeleted == false)
        //includes[]=[Include(x=>x.Category), Include(x=>x.Brand)]
        /* query = context
        .Products
        .Where(p=>p.IsDeleted==false)
        .Include(x=>x.Cateogry)
        .Include(x=>x.Brand)
        */
    }

    public void Update(TEntity entity)
    {
        throw new NotImplementedException();
    }
}
