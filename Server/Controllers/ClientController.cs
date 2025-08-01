using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController:GenericController<Client>
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
        : base(clientService) 
    {
        _clientService = clientService;
    }
}
