using Microsoft.AspNetCore.Mvc;

namespace app_dotnet.Controllers;

[ApiController]
[Route("api/saludo")]
public class SaludoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Content("Hola mundo", "text/plain");
    }
}
