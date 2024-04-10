using BookTalk.Shared.Common;
using BookTalk.Shared.Exceptions;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace BookTalk.Client.Controllers;

[Route("[controller]")]
public class ReviewController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly string _baseApiUrl;


    public ReviewController(IConfiguration configuration)
    {
        _configuration = configuration;
        _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
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
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
        }

        return View(model);
    }


    [Route("Post")]
    [HttpPost]
    public IActionResult Post(ReviewPostViewModel viewModel)
    {
        ResponseMessage responseData = new ResponseMessage();
        ReviewPostViewModel model = new ReviewPostViewModel();
        string url;

        try
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId))
                {
                    
                    viewModel.SessionId = sessionId;
                    url = Utility.GetEndpointUrl(_baseApiUrl, "Review", "Post");
                    HttpClient client = new HttpClient();
                    var response = client.PostAsJsonAsync(url, viewModel).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        responseData = JsonConvert.DeserializeObject<ResponseMessage>(content);
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
                    return RedirectToAction("Signin");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {                    
                    string errorMessage = error.ErrorMessage;
                    Console.WriteLine(errorMessage);
                }


                ViewBag.TinyMCEApiKey = _configuration.GetValue<string>("TinyMCE:ApiKey");
                model.Rates = GetRates();
                return View(model);
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
            responseData.ErrorMessage = ex.Message;

            ViewBag.TinyMCEApiKey = _configuration.GetValue<string>("TinyMCE:ApiKey");
            model.Rates = GetRates();
            return View(model);
        }
    }


    //[Route("Create")]
    //[HttpGet]
    //public IActionResult Create()
    //{
    //    ReviewPostViewModel model = new ReviewPostViewModel();

    //    try
    //    {
    //        HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId);
    //        if (string.IsNullOrWhiteSpace(sessionId))
    //        {
    //            return RedirectToAction("Signin", "Account");
    //        }

    //        ViewBag.TinyMCEApiKey = _configuration.GetValue<string>("TinyMCE:ApiKey");
    //        model.Rates = GetRates();
    //    }
    //    catch (Exception ex)
    //    {
    //        ViewBag.ErrorMessage = Utility.GetMessage("msg01");
    //    }

    //    return View(model);
    //}


    //[Route("Create")]
    //[HttpPost]
    //public IActionResult Create(ReviewPostViewModel viewModel)
    //{
    //    ResponseMessage responseData = new ResponseMessage();
    //    ReviewPostViewModel model = new ReviewPostViewModel();
    //    string url;

    //    try
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            if (HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId))
    //            {
    //                viewModel.SessionId = sessionId;
    //                url = Utility.GetEndpointUrl(_baseApiUrl, "Review", "Create");
    //                HttpClient client = new HttpClient();
    //                var response = client.PostAsJsonAsync(url, viewModel).Result;
    //                var content = response.Content.ReadAsStringAsync().Result;

    //                if (response.IsSuccessStatusCode)
    //                {
    //                    responseData = JsonConvert.DeserializeObject<ResponseMessage>(content);
    //                    return RedirectToAction("Index");
    //                }
    //                else
    //                {
    //                    responseData.ErrorCode = response.StatusCode.ToString();
    //                    throw new Exception(responseData.ErrorMessage);
    //                }
    //            }
    //            else
    //            {
    //                return RedirectToAction("Signin");
    //            }
    //        }
    //        else
    //        {
    //            ViewBag.TinyMCEApiKey = _configuration.GetValue<string>("TinyMCE:ApiKey");
    //            model.Rates = GetRates();
    //            return View(model);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ViewBag.ErrorMessage = Utility.GetMessage("msg01");
    //        responseData.ErrorMessage = ex.Message;

    //        ViewBag.TinyMCEApiKey = _configuration.GetValue<string>("TinyMCE:ApiKey");
    //        model.Rates = GetRates();
    //        return View(model);
    //    }
    //}


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
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
        }
        finally
        {
            if (responseData.Data == null)
            {
                responseData.Data = new ReviewViewModel();
            }
        }

        return View(responseData.Data);
    }


    [Route("CreateComment")]
    [HttpPost]
    public IActionResult CreateComment([FromBody] Comment comment)
    {
        ResponseMessage<Comment> responseData = new ResponseMessage<Comment>();
        CommentViewModel viewModel;
        string url;

        try
        {
            HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId);
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return Unauthorized(new { Message = Utility.GetMessage("msg08") });
            }

            viewModel = new CommentViewModel()
            {
                ReviewId = comment.ReviewId,
                CommentId = comment.CommentId,
                SessionId = sessionId,
                Content = comment.Content
            };

            url = Utility.GetEndpointUrl(_baseApiUrl, "Review", "CreateComment");
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync(url, viewModel).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            responseData = JsonConvert.DeserializeObject<ResponseMessage<Comment>>(content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(responseData.Data == null ? new Comment() : responseData.Data);
            }
            else
            {
                responseData.ErrorCode = Convert.ToInt32(response.StatusCode).ToString();
                throw new Exception(responseData.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(Convert.ToInt32(responseData.ErrorCode), new { message = Utility.GetMessage("msg01") });
        }
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
