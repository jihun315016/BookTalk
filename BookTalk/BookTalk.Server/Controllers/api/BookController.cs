using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using BookTalk.Shared.Utility;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Server.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("GetList")]
        [HttpPost]
        public IActionResult GetList([FromBody] BookQuery bookQuery)
        {
            string baseUrl;
            string key;
            string url;
            ResponseMessage<IEnumerable<BookQuery>> responseData = new ResponseMessage<IEnumerable<BookQuery>>();

            try
            {
                key = _configuration["Aladin:Key"];
                baseUrl = _configuration["Aladin:List:Url"];
                bookQuery.QueryType = "ItemNewAll";
                bookQuery.MaxResult = Convert.ToInt32(_configuration["Aladin:List:MaxResult"]);
                bookQuery.SearchTarget = _configuration["Aladin:List:SearchTarget"];
                bookQuery.Output = _configuration["Aladin:List:Output"];

                BookService bookService = new BookService();
                url = bookService.GetUrlForNewOrBestSellerBooks(baseUrl, key, bookQuery);

                // 주목할 만한 신간 리스트
                BookQuery resData1 = bookService.GetBooks(url);

                // 베스트셀러
                bookQuery.QueryType = "BlogBest";
                bookQuery.Item = new List<Book>();
                url = bookService.GetUrlForNewOrBestSellerBooks(baseUrl, key, bookQuery);
                BookQuery resData2 = bookService.GetBooks(url);

                
                responseData.Data = new List<BookQuery>() { resData1, resData2 }; 
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
        public IActionResult SearchList([FromBody] BookQuery bookQuery)
        {
            string baseUrl;
            string key;
            string url;
            ResponseMessage<BookQuery> responseData = new ResponseMessage<BookQuery>();

            try
            {
                key = _configuration["Aladin:Key"];
                baseUrl = _configuration["Aladin:Search:Url"];
                bookQuery.QueryType = "Title";
                bookQuery.MaxResult = Convert.ToInt32(_configuration["Aladin:List:MaxResult"]);
                bookQuery.SearchTarget = _configuration["Aladin:List:SearchTarget"];
                bookQuery.Output = _configuration["Aladin:List:Output"];

                BookService bookService = new BookService();
                url = bookService.GetUrlForBookSearch(baseUrl, key, bookQuery);
                responseData.Data = bookService.GetBooks(url);

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
