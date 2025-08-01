using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentResourceController : GenericController<ShipmentResource>
{
    private readonly IShipmentResourceService _service;
    public ShipmentResourceController(IShipmentResourceService service) : base(service)
    {
        _service = service;
    }

    [HttpPut("{id}/quantity")]
    public async Task<IActionResult> UpdateQuantity(Guid id, [FromBody] decimal quantity)
    {
        var result = await _service.UpdateResourceQuantityAsync(id, quantity);
        return result ? Ok() : BadRequest("Ошибка при обновлении количества.");
    }
}