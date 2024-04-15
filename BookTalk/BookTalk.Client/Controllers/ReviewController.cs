using BookTalk.Shared.Common;
using BookTalk.Shared.Exceptions;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;

namespace BookTalk.Client.Controllers;

[Route("[controller]")]
public class ReviewController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ReviewController> _logger;
    private readonly string _baseApiUrl;

    public ReviewController(IConfiguration configuration, ILogger<ReviewController> logger)
    {
        _configuration = configuration;
        _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        _logger = logger;
    }


    public IActionResult Index(string? queryType, string? keyword)
    {
        ResponseMessage<ReviewIndexViewModel> responseData = new ResponseMessage<ReviewIndexViewModel>();
        ReviewIndexViewModel model = new ReviewIndexViewModel();
        string url;
        
        try
        {
            model.QueryType = string.IsNullOrWhiteSpace(queryType) ? "" : queryType;
            model.Keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword;
            model.Items = new List<ReviewViewModel>();

            url = Utility.GetEndpointUrl(_baseApiUrl, "Review", "Search");
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync(url, model).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                responseData = JsonConvert.DeserializeObject<ResponseMessage<ReviewIndexViewModel>>(content);
            }
            else
            {
                responseData.ErrorCode = response.StatusCode.ToString();
                throw new Exception(responseData.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteError(_logger, ex);
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
        }

        return View(responseData.Data);
    }


    [Route("Post")]
    [HttpGet]
    public IActionResult Post()
    {
        ReviewPostViewModel model = new ReviewPostViewModel();

        try
        {
            HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId);
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return RedirectToAction("Signin", "Account");
            }

            ViewBag.TinyMCEApiKey = _configuration.GetValue<string>("TinyMCE:ApiKey");
            model.Rates = GetRates();
        }
        catch (Exception ex)
        {
            Logging.WriteError(_logger, ex);
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
        }

        return View(model);
    }


    [Route("Post")]
    [HttpPost]
    public IActionResult Post(ReviewPostViewModel viewModel)
    {
        ResponseMessage responseData = new ResponseMessage();
        string url;

        try
        {
            ViewBag.TinyMCEApiKey = _configuration.GetValue<string>("TinyMCE:ApiKey");
            viewModel.Rates = GetRates();

            if (ModelState.IsValid)
            {
                if (HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId))
                {
                    
                    viewModel.SessionId = sessionId;
                    url = Utility.GetEndpointUrl(_baseApiUrl, "Review", "Post");
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
                        if (response.StatusCode == HttpStatusCode.BadRequest && responseData.ErrorCode == Utility.GetUserStatusCodeNumber(UserStatusCode.OverlapException))
                        {
                            responseData.ErrorCode = response.StatusCode.ToString();
                            throw new UserOverlapException(responseData.ErrorMessage);
                        }
                        else
                        {
                            responseData.ErrorCode = response.StatusCode.ToString();
                            throw new Exception(responseData.ErrorMessage);
                        }                        
                    }
                }
                else
                {
                    return RedirectToAction("Signin");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    string errorMessage = error.ErrorMessage;
                    Logging.WriteError(_logger);
                }

                return View(viewModel);
            }
        }
        catch (UserOverlapException ex)
        {
            ViewBag.ErrorMessage = Utility.GetMessage("msg09");
            Logging.WriteError(_logger, ex);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
            responseData.ErrorMessage = ex.Message;
            return View(viewModel);
        }
    }


    [Route("{id}")]
    [HttpGet]

    public IActionResult Read(int id)
    {
        ResponseMessage<ReviewViewModel> responseData = new ResponseMessage<ReviewViewModel>();
        ReviewViewModel model;
        string url;

        try
        {
            HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId);

            model = new ReviewViewModel()
            {
                Id = id,
                Title = "",
                Author = "",
                UserId = "",
                BookName = "",
                Cover = "",
                CurrentSessionId = sessionId ?? "",
                Page = new Pagination()
            };

            url = Utility.GetEndpointUrl(_baseApiUrl, "Review", "Read");
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync(url, model).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            responseData = JsonConvert.DeserializeObject<ResponseMessage<ReviewViewModel>>(content);

            ViewBag.baseUrl = _baseApiUrl;

            if (!response.IsSuccessStatusCode)
            {
                if (string.IsNullOrWhiteSpace(responseData.ErrorCode)) 
                {
                    responseData.ErrorCode = Convert.ToInt32(response.StatusCode).ToString();
                }

                if(responseData.ErrorCode == Utility.GetUserStatusCodeNumber(UserStatusCode.NoDataException))
                {
                    // 작성되지 않은 리뷰를 요청하면 조회 페이지로 이동
                    throw new UserNoDataException();
                }
                else
                {
                    throw new Exception(responseData.ErrorMessage);
                }
            }
        }
        catch (UserNoDataException ex)
        {
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Logging.WriteError(_logger, ex);
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
        }

        return View(responseData.Data);
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
