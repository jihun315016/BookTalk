using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Server.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AladinController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AladinController(IConfiguration configuration)
        {
            _configuration = configuration;
            // var exam = _configuration["Aladin:SearchUrl"]
        }
    }
}
