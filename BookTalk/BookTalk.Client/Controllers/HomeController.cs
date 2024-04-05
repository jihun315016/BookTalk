using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using BookTalk.Shared.Utility;
using BookTalk.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BookTalk.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly string _baseApiUrl;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ResponseMessage<IEnumerable<BookListQuery>> responseData = new ResponseMessage<IEnumerable<BookListQuery>>();
        BookListQuery bookQuery = new BookListQuery();
        string url;

        try
        {
            url = Utility.GetEndpointUrl(_baseApiUrl, "Book", "GetList");
            HttpClient client = new HttpClient();
            var response = await client.PostAsJsonAsync<BookListQuery>(url, bookQuery);
            string content = await response.Content.ReadAsStringAsync();
            
            if(response.IsSuccessStatusCode)
            {
                responseData = JsonConvert.DeserializeObject<ResponseMessage<IEnumerable<BookListQuery>>>(content);
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
            responseData.InitializeResponseMessage(ex.Message, null);

            // responseData.DeveloperErrorMessage ·Î±ë
        }

        return View(responseData.Data);
    }
}
