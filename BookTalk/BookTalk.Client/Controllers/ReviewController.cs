﻿using BookTalk.Shared.Common;
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


    [Route("Create")]
    [HttpGet]
    public IActionResult Create()
    {
        ReviewCreateViewModel model = new ReviewCreateViewModel();

        HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId);
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            return RedirectToAction("Signin", "Account");
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


    [Route("Create")]
    [HttpPost]
    public IActionResult Create(ReviewCreateViewModel viewModel)
    {
        ResponseMessage responseData = new ResponseMessage();
        ReviewCreateViewModel model = new ReviewCreateViewModel();
        string url;

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


    [Route("{id}")]
    [HttpGet]

    public IActionResult Read(int id)
    {
        ResponseMessage<ReviewViewModel> responseData = new ResponseMessage<ReviewViewModel>();
        ReviewViewModel model;
        string url;

        try
        {
            model = new ReviewViewModel()
            {
                Id = id,
                Title = "",
                Author = "",
                UserId = "",
                BookName = "",
                Cover = ""
            };

            url = Utility.GetEndpointUrl(_baseApiUrl, "Review", "Read");
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync(url, model).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                responseData = JsonConvert.DeserializeObject<ResponseMessage<ReviewViewModel>>(content);
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
        finally
        {
            if (responseData.Data == null)
            {
                responseData.Data = new ReviewViewModel();
            }
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
