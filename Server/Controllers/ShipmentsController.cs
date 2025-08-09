using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentsController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpPost("filter")]
        public async Task<ActionResult<List<ShipmentDocumentDto>>> Filter(ShipmentFilterDto filter)
        {
            return await _shipmentService.FilterAsync(filter);
        }
    }
}
