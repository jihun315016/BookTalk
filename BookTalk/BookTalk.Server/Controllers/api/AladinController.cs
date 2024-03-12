using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Server.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AladinController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AladinController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// https://localhost:xxxx/api/Aladin/DefaultDisplay
        /// </summary>
        /// <param name="aladinBookQuery"></param>
        /// <returns></returns>
        /*
        {
            "title": "",
            "author": "",
            "queryType": "ItemNewAll",
            "categoryId": "0",
            "searchTarget": "Book",
            "output": "js",
            "start": 1,
            "maxResult": 5,
            "item": [],
            "ErrorMessage": "",
            "ErrorCode": ""
        }
         */
        [Route("DefaultDisplay")]
        [HttpPost]
        public async Task<IActionResult> DefaultDisplay([FromBody] AladinBookQuery aladinBookQuery)
        {
            string baseUrl;
            string key;
            string url;
            ResponseMessage<IEnumerable<AladinBookQuery>> resData = new ResponseMessage<IEnumerable<AladinBookQuery>>();

            try
            {
                key = _configuration["Aladin:Key"];
                baseUrl = _configuration["Aladin:List:Url"];
                aladinBookQuery.QueryType = "ItemNewAll";
                aladinBookQuery.MaxResult = Convert.ToInt32(_configuration["Aladin:List:MaxResult"]);
                aladinBookQuery.SearchTarget = _configuration["Aladin:List:SearchTarget"];
                aladinBookQuery.Output = _configuration["Aladin:List:Output"];

                AladinService aladinService = new AladinService();
                url = aladinService.GetUrlBookList(baseUrl, key, aladinBookQuery);

                // 주목할 만한 신간 리스트
                AladinBookQuery resData1 = await aladinService.GetBooks(url);

                // 베스트셀러
                aladinBookQuery.QueryType = "BlogBest";
                aladinBookQuery.Item = new List<AladinBook>();
                url = aladinService.GetUrlBookList(baseUrl, key, aladinBookQuery);
                AladinBookQuery resData2 = await aladinService.GetBooks(url);

                
                resData.Data = new List<AladinBookQuery>() { resData1, resData2 }; 
                return Ok(resData);
            }
            catch (Exception ex)
            {
                resData.ErrorCode = "-1";
                resData.DeveloperErrorMessage = ex.Message;
            }

            return StatusCode(StatusCodes.Status500InternalServerError, resData);
        }

        [Route("SearchBooks")]
        [HttpPost]
        public IActionResult SearchBooks()
        {
            return null;
        }
    }
}
