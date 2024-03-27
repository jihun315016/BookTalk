using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using BookTalk.Shared.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            "query": "",
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
        [Route("GetList")]
        [HttpPost]
        public IActionResult GetList([FromBody] AladinBookQuery aladinBookQuery)
        {
            string baseUrl;
            string key;
            string url;
            ResponseMessage<IEnumerable<AladinBookQuery>> responseData = new ResponseMessage<IEnumerable<AladinBookQuery>>();

            try
            {
                key = _configuration["Aladin:Key"];
                baseUrl = _configuration["Aladin:List:Url"];
                aladinBookQuery.QueryType = "ItemNewAll";
                aladinBookQuery.MaxResult = Convert.ToInt32(_configuration["Aladin:List:MaxResult"]);
                aladinBookQuery.SearchTarget = _configuration["Aladin:List:SearchTarget"];
                aladinBookQuery.Output = _configuration["Aladin:List:Output"];

                AladinService aladinService = new AladinService();
                url = aladinService.GetUrlForNewOrBestSellerBooks(baseUrl, key, aladinBookQuery);

                // 주목할 만한 신간 리스트
                AladinBookQuery resData1 = aladinService.GetBooks(url);

                // 베스트셀러
                aladinBookQuery.QueryType = "BlogBest";
                aladinBookQuery.Item = new List<AladinBook>();
                url = aladinService.GetUrlForNewOrBestSellerBooks(baseUrl, key, aladinBookQuery);
                AladinBookQuery resData2 = aladinService.GetBooks(url);

                
                responseData.Data = new List<AladinBookQuery>() { resData1, resData2 }; 
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
                responseData.ErrorMessage = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseData);
            }

        }

        [Route("SearchList")]
        [HttpPost]
        public IActionResult SearchList([FromBody] AladinBookQuery aladinBookQuery)
        {
            string baseUrl;
            string key;
            string url;
            ResponseMessage<AladinBookQuery> responseData = new ResponseMessage<AladinBookQuery>();

            try
            {
                key = _configuration["Aladin:Key"];
                baseUrl = _configuration["Aladin:Search:Url"];
                aladinBookQuery.QueryType = "Title";
                aladinBookQuery.MaxResult = Convert.ToInt32(_configuration["Aladin:List:MaxResult"]);
                aladinBookQuery.SearchTarget = _configuration["Aladin:List:SearchTarget"];
                aladinBookQuery.Output = _configuration["Aladin:List:Output"];

                AladinService aladinService = new AladinService();
                url = aladinService.GetUrlForBookSearch(baseUrl, key, aladinBookQuery);
                responseData.Data = aladinService.GetBooks(url);

                return Ok(responseData);
            }
            catch (Exception ex)
            {
                responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
                responseData.ErrorMessage = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseData);
            }
        }
    }
}
