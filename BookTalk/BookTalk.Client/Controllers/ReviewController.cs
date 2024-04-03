using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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
        if (HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId))
        {

        }

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
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(ReviewCreateViewModel viewModel)
    {
        ResponseMessage responseData = new ResponseMessage();
        ReviewCreateViewModel model = new ReviewCreateViewModel();
        string url;
        string apiKey;

        try
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId))
                {
                    viewModel.SessionId = sessionId;
                    url = Utility.GetEndpointUrl(_baseApiUrl, "Review", "Create");
                    HttpClient client = new HttpClient();
                    var response = client.PostAsJsonAsync(url, viewModel).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    responseData = JsonConvert.DeserializeObject<ResponseMessage>(content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        responseData.ErrorCode = response.StatusCode.ToString();
                        throw new Exception(responseData.ErrorMessage);
                    }
                }
                else
                {
                    // 로그인 풀렸으니까 그거 처리
                }

            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
            responseData.ErrorMessage = ex.Message;
        }

        apiKey = _configuration.GetValue<string>("TinyMCE:ApiKey");
        ViewBag.TinyMCEApiKey = apiKey;
        model.Rates = new SelectList(new[] { 1, 2, 3, 4, 5 });
        return View(model);
    }
}

