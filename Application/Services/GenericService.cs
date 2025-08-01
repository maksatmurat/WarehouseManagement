using Application.Interfaces;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
{
    protected readonly IGenericRepository<TEntity> _repository;

    public GenericService(IGenericRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public virtual async Task<List<TEntity>> GetAllAsync() =>
        await _repository.GetAllAsync();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id) =>
        await _repository.GetByIdAsync(id);

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) throw new KeyNotFoundException("Entity not found.");

        _repository.SetValues(existing,entity);
        await _repository.SaveChangesAsync();
        return existing;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return false;

        _repository.Remove(existing);
        await _repository.SaveChangesAsync();
        return true;
    }
}