using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenericController<TEntity> : ControllerBase where TEntity : class
{
    private readonly IGenericService<TEntity> _service;
    public GenericController(IGenericService<TEntity> service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var entities = await _service.GetAllAsync();

        // Проверяем, есть ли у типа свойство IsActive
        var prop = typeof(TEntity).GetProperty("IsActive", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (prop == null)
            return BadRequest("У этой сущности нет свойства IsActive");

        var activeEntities = entities
            .Where(e => prop.GetValue(e) is bool b && b)
            .ToList();

        return Ok(activeEntities);
    }
    [HttpPost]
    public async Task<IActionResult> Create(TEntity entity)
    {
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(Get), new { id = GetEntityId(created) }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TEntity entity)
    {
        var updated = await _service.UpdateAsync(id, entity);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }

    private object GetEntityId(TEntity entity)
    {
        var prop = typeof(TEntity).GetProperty("Id");
        return prop?.GetValue(entity) ?? Guid.Empty;
    }
}
