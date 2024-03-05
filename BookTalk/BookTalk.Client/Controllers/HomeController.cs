using BookTalk.Shared.Temps;
using BookTalk.Shared.Temps.Aladin;
using BookTalk.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            AladinBookQuery aladinBookQuery = new AladinBookQuery()
            {
                Start = 1,
                QueryType = "",
                CategoryId = "0",
            };

            ResponseMessage<AladinBookQuery> data = new ResponseMessage<AladinBookQuery>()
            {
                Data = aladinBookQuery
            };


            try
            {

                HttpClient client = new HttpClient();
                var response = await client.PostAsJsonAsync<ResponseMessage<AladinBookQuery>>("", data);

                if (response.IsSuccessStatusCode) 
                {

                }
                else
                {
                    data.ErrorCode = Convert.ToInt32(response.StatusCode);
                    data.ErrorMessage = "";
                }

                return View();
            }
            catch (Exception ex)
            {
                data.ErrorCode = -1;
                data.ErrorMessage = ex.Message;
                return View();
            }
        }



        [HttpPost]
        public IActionResult Index(AladinBookQuery aladinBookQuery)
        {
            if (aladinBookQuery != null)
            {
                aladinBookQuery.CategoryId = "0"; // 카테고리 전체 조회
            }

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
