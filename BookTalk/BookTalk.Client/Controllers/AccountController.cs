using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
