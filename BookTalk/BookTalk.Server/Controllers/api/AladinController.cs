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
            "item": []
        }
         */
        [Route("DefaultDisplay")]
        [HttpPost]
        public async Task<IActionResult> DefaultDisplay([FromBody] AladinBookQuery aladinBookQuery)
        {
            string url; 
            string key;

            try
            {
                key = _configuration["Aladin:Key"];

                aladinBookQuery.MaxResult = Convert.ToInt32(_configuration["Aladin:List:MaxResult"]);
                aladinBookQuery.SearchTarget = _configuration["Aladin:List:SearchTarget"];
                aladinBookQuery.Output = _configuration["Aladin:List:Output"];

                string query = $"ttbkey={key}&QueryType={aladinBookQuery.QueryType}&CategoryId={aladinBookQuery.CategoryId}" +
                    $"&MaxResults={aladinBookQuery.MaxResult}&start={aladinBookQuery.Start}&SearchTarget={aladinBookQuery.SearchTarget}" +
                    $"&cover={aladinBookQuery.Cover}&output={aladinBookQuery.Output}&Version=20131101";
                url = _configuration["Aladin:List:Url"] + query;

                AladinService aladinService = new AladinService();
                ResponseMessage<AladinBookQuery> resData = await aladinService.GetBooks(url);
                return Ok(resData);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("SearchBooks")]
        [HttpPost]
        public IActionResult SearchBooks()
        {
            return null;
        }
    }
}
