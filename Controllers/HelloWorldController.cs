using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HelloWorldController : ControllerBase
{

    /// <summary>
    /// Hello World (Login Jwt)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Get()
    {

        return Ok("Hello World");
    }
}

