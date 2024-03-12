using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Server.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("GetMenuList")]
        public IActionResult GetMenuList()
        {
            return Ok();
        }
    }
}
