using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Client.Controllers;

public class UserController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
