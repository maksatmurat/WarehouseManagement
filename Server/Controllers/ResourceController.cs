using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourceController:GenericController<Resource>
{
    public ResourceController(IGenericService<Resource> service):base(service)
    {
    }
}
