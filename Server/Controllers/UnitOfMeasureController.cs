using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnitOfMeasureController:GenericController<UnitOfMeasure>
{
    private readonly IUnitOfMeasureService _unitOfMeasureService;

    public UnitOfMeasureController(IUnitOfMeasureService unitOfMeasureService)
        : base(unitOfMeasureService)
    {
        _unitOfMeasureService = unitOfMeasureService;
    }
}
