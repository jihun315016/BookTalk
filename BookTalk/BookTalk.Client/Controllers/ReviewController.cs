using BookTalk.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookTalk.Client.Controllers;

public class ReviewController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly string _baseApiUrl;

    public ReviewController(IConfiguration configuration)
    {
        _configuration = configuration;
        _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        ReviewCreateViewModel model = new ReviewCreateViewModel();
        string apiKey;

        try
        {
            apiKey = _configuration.GetValue<string>("TinyMCE:ApiKey");
            ViewBag.TinyMCEApiKey = apiKey;
            model.Rates = new SelectList(new[] { 1, 2, 3, 4, 5 });

        }
        catch (Exception ex)
        {

        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(ReviewCreateViewModel viewModel)
    {
        try
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
        }
        catch (Exception ex)
        {

        }
        return View();
    }
}

