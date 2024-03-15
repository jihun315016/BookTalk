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
        ResponseMessage<IEnumerable<AladinBookQuery>> responseData = new ResponseMessage<IEnumerable<AladinBookQuery>>();
        string url;

        try
        {
            AladinBookQuery aladinBookQuery = new AladinBookQuery()
            {
                QueryType = "",
                Start = 1,
                MaxResult = 6,
                Item = new List<AladinBook>()
            };

            url = Utility.GetEndpointUrl(_baseApiUrl, "Aladin", "GetList");
            HttpClient client = new HttpClient();
            var response = await client.PostAsJsonAsync<AladinBookQuery>(url, aladinBookQuery);
            string content = await response.Content.ReadAsStringAsync();
            
            if(response.IsSuccessStatusCode)
            {
                responseData = JsonConvert.DeserializeObject<ResponseMessage<IEnumerable<AladinBookQuery>>>(content);
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



    [HttpPost]
    public IActionResult Index(AladinBookQuery aladinBookQuery)
    {
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
