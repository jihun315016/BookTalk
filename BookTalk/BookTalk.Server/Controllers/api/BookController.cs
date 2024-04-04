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
        private readonly BookService _bookService;
        private readonly IConfiguration _configuration;

        public BookController(BookService bookService, IConfiguration configuration)
        {
            _bookService = bookService;
            _configuration = configuration;
        }

        [Route("GetList")]
        [HttpPost]
        public IActionResult GetList([FromBody] BookListQuery bookQuery)
        {
            string key;
            string url;
            ResponseMessage<IEnumerable<BookListQuery>> responseData = new ResponseMessage<IEnumerable<BookListQuery>>();

            try
            {
                key = _configuration["Aladin:Key"];
                _bookService.SetBookList(bookQuery, _configuration["Aladin:ListType"], "ItemNewAll");

                url = _bookService.GetUrlForNewOrBestSellerBooks(bookQuery.BaseUrl, key, bookQuery);

                // 주목할 만한 신간 리스트
                BookListQuery resData1 = _bookService.GetBooks(url);

                // 베스트셀러
                _bookService.SetBookList(bookQuery, _configuration["Aladin:ListType"], "BlogBest");
                url = _bookService.GetUrlForNewOrBestSellerBooks(bookQuery.BaseUrl, key, bookQuery);
                BookListQuery resData2 = _bookService.GetBooks(url);

                
                responseData.Data = new List<BookListQuery>() { resData1, resData2 }; 
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
        public IActionResult SearchList([FromBody] BookListQuery bookQuery)
        {
            ResponseMessage<BookListQuery> responseData = new ResponseMessage<BookListQuery>();
            string key;
            string url;

            try
            {
                key = _configuration["Aladin:Key"];

                // 검색어가 없으면 검색 API 호출 시 오류 -> 검색어 없으면 베스트 셀러 조회
                if (string.IsNullOrWhiteSpace(bookQuery.Query))
                {
                    _bookService.SetBookList(bookQuery, _configuration["Aladin:ListType"], "BlogBest");
                    url = _bookService.GetUrlForNewOrBestSellerBooks(bookQuery.BaseUrl, key, bookQuery);
                }
                else
                {
                    _bookService.SetBookSearch(bookQuery, _configuration["Aladin:SearchType"]);
                    url = _bookService.GetUrlForBookSearch(bookQuery.BaseUrl, key, bookQuery);
                }

                responseData.Data = _bookService.GetBooks(url);
                responseData.Data.MinPage = bookQuery.MinPage;
                responseData.Data.MaxPage = bookQuery.MaxPage;
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
                responseData.ErrorMessage = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseData);
            }
        }


        [Route("GetOne")]
        [HttpPost]
        public IActionResult GetOne([FromBody] BookListQuery bookQuery)
        {
            ResponseMessage<BookListQuery> responseData = new ResponseMessage<BookListQuery>();
            string key;
            string url;

            try
            {
                key = _configuration["Aladin:Key"];
                // DetailType
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
