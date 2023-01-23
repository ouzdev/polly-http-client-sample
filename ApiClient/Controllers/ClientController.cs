using ApiClient.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ApiClient.Controllers;

[Route("api")]
public class ClientController : BaseController
{
    /// <summary>
    /// Say Hello
    /// </summary>
    /// <returns></returns>
    [HttpGet("hello")]
    public async Task<IActionResult> GetSayHello()
    {
        await Mediator.Send(new GetSayHelloQuery());
        return Ok("Hello Word!");
    }
}