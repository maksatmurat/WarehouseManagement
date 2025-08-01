//using Infrastructure.Repositories.Contracts;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;

//namespace Server.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//public class GenericController<T>(IGenericRepository<T> genericRepositoryInterface) :
//        Controller where T : class
//{
//    [HttpGet("all")]
//    public async Task<IActionResult> GetAll() => Ok(await genericRepositoryInterface.GetAll());

//    [HttpDelete("delete/{id}")]
//    public async Task<IActionResult> DeleteById(int id)
//    {
//        var result = await genericRepositoryInterface.DeleteById(id);
//        return Ok(result);
//    }

//    [HttpGet("single/{id}")]
//    public async Task<IActionResult> GetById(int id)
//    {
//        if (id <= 0) return BadRequest("Invalid ID provided.");
//        return Ok(await genericRepositoryInterface.GetById(id));
//    }
//    [HttpPost("add")]
//    public async Task<IActionResult> Add(T model)
//    {
//        if (model is null) return BadRequest("Bad request made");
//        return Ok(await genericRepositoryInterface.Insert(model));
//    }
//    [HttpPut("update")]
//    public async Task<IActionResult> Update(T model)
//    {
//        if (model is null) return BadRequest("Bad request made");
//        return Ok(await genericRepositoryInterface.Update(model));
//    }
//}
