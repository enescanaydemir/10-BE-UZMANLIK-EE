using System;
using EShop.Data.Abstract;
using EShop.Data.Concrete.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Data.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly EShopDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    public void Dispose() //dispose metodu ne işe yarar ? bellekten temizleme işlemi yapar
    {
        _dbContext.Dispose();
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        var repository = _serviceProvider.GetRequiredService<IGenericRepository<TEntity>>();
        return repository;
    }

    public int Save()
    {
        return _dbContext.SaveChanges();
    }

    public async Task<int> SaveAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
