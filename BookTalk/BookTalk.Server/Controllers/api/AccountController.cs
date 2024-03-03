using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Server.Controllers.api;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost]
    [Route("Login")]
    public IActionResult Login()
    {
        return Ok();
    }
}
