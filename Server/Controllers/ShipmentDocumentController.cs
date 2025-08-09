using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentDocumentController : GenericController<ShipmentDocument>
{
    private readonly IShipmentDocumentService _service;

    public ShipmentDocumentController(IShipmentDocumentService service) : base(service)
    {
        _service = service;
    }

    //[HttpPost("{id}/sign")]
    //public async Task<IActionResult> SignDocument(Guid id)
    //{
    //    var result = await _service.SignDocumentAsync(id);
    //    return result ? Ok() : BadRequest("Ошибка при подписании документа.");
    //}

    //[HttpPost("{id}/cancel")]
    //public async Task<IActionResult> CancelDocument(Guid id)
    //{
    //    var result = await _service.CancelDocumentAsync(id);
    //    return result ? Ok() : BadRequest("Ошибка при отмене документа.");
    //}
}
