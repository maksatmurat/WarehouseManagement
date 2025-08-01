using Domain.Entities;

namespace Application.Interfaces;

public interface IUnitOfMeasureService : IGenericService<UnitOfMeasure>
{
    Task<List<UnitOfMeasure>> GetActiveUnitsAsync();
}
