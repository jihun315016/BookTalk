using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using BookTalk.Shared.Utility;
using BookTalk.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BookTalk.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                AladinBookQuery aladinBookQuery = new AladinBookQuery()
                {
                    QueryType = "ItemNewAll",
                    CategoryId = "0",
                    SearchTarget = "Book",
                    Output = "js",
                    Start = 1,
                    MaxResult = 5,
                    Cover= "Big",
                    Item = new List<AladinBook>()
                };

                HttpClient client = new HttpClient();
                var response = await client.PostAsJsonAsync<AladinBookQuery>("https://localhost:7033/api/Aladin/DefaultDisplay", aladinBookQuery);
                ResponseMessage<AladinBookQuery> responseData = new ResponseMessage<AladinBookQuery>();

                if (response.IsSuccessStatusCode) 
                {
                    string content = await response.Content.ReadAsStringAsync();
                    responseData = JsonConvert.DeserializeObject<ResponseMessage<AladinBookQuery>>(content);
                }
                else
                {
                    if (responseData.ErrorCode == default(int))
                    {
                        responseData.ErrorCode = Convert.ToInt32(response.StatusCode);
                    }

                    if (string.IsNullOrWhiteSpace(responseData.ErrorMessage)) 
                    {
                        responseData.ErrorMessage = Utility.GetMessage("Msg01");
                    }

                    throw new Exception($"[{responseData.ErrorCode}] {responseData.ErrorMessage}");
                }

                return View(responseData);
            }
            catch (Exception ex)
            {
                return View();
            }
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
}
