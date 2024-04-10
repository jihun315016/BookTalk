using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace BookTalk.Client.ViewComponents.Review;

public class ReviewPostViewComponent : ViewComponent
{
    private readonly IConfiguration _configuration;

    public ReviewPostViewComponent(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IViewComponentResult Invoke()
    {
        ReviewPostViewModel model = new ReviewPostViewModel();

        HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId);
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            return View();
        }

        try
        {
            ViewBag.TinyMCEApiKey = _configuration.GetValue<string>("TinyMCE:ApiKey");
            model.Rates = GetRates();
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
        }

        return View(model);
    }


    private IEnumerable<SelectListItem> GetRates()
    {
        List<SelectListItem> rateItems = new List<SelectListItem>();
        int minRate = _configuration.GetValue<int>("Review:minRate");
        int maxRate = _configuration.GetValue<int>("Review:maxRate");

        for (int i = minRate; i <= maxRate; i++)
        {
            rateItems.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
        }
        return rateItems.OrderByDescending(r => r.Text);
    }
}
