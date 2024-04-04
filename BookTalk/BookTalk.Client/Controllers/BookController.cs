using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using BookTalk.Shared.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BookTalk.Client.Controllers
{
    public class BookController : Controller
    {
        private readonly string _baseApiUrl;

        public BookController(IConfiguration configuration)
        {
            _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }

        [HttpGet]
        public IActionResult SearchList(string? keyword, int page = 1)
        {
            ResponseMessage<BookListQuery> responseData = new ResponseMessage<BookListQuery>();
            string url;

            try
            {
                responseData = GetSearchBooks(keyword, page);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = Utility.GetMessage("msg01");
            }

            return View(responseData.Data);
        }


        [HttpGet]
        public async Task<IActionResult> SearchListJson(string? keyword, int page = 1)
        {
            ResponseMessage<BookListQuery> responseData = new ResponseMessage<BookListQuery>();
            string url;

            try
            {
                responseData = GetSearchBooks(keyword, page);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = Utility.GetMessage("msg01");
                return StatusCode(StatusCodes.Status500InternalServerError, responseData);
            }

            return Json(responseData.Data);
        }


        [HttpGet]
        public IActionResult GetOne(string type, string isbn)
        {
            ResponseMessage<BookDetailQuery> responseData = new ResponseMessage<BookDetailQuery>();
            string url;

            try
            {
                BookDetailQuery bookQuery = new BookDetailQuery()
                {
                    ItemIdType = type,
                    ItemId = isbn
                };

                url = Utility.GetEndpointUrl(_baseApiUrl, "Book", "GetOne");
                HttpClient client = new HttpClient();
                var response = client.PostAsJsonAsync<BookDetailQuery>(url, bookQuery).Result;
                string content = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = JsonConvert.DeserializeObject<ResponseMessage<BookDetailQuery>>(content);
                    responseData.Data.ItemIdType = bookQuery.ItemIdType;
                    responseData.Data.ItemId = bookQuery.ItemId;
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


        private ResponseMessage<BookListQuery> GetSearchBooks(string keyword, int page)
        {
            ResponseMessage<BookListQuery> responseData = new ResponseMessage<BookListQuery>();
            string url;

            try
            {
                BookListQuery bookQuery = new BookListQuery()
                {
                    Query = string.IsNullOrWhiteSpace(keyword) ? "" : keyword,
                    Keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword,
                    Page = page
                };

                url = Utility.GetEndpointUrl(_baseApiUrl, "Book", "SearchList");
                HttpClient client = new HttpClient();
                var response = client.PostAsJsonAsync<BookListQuery>(url, bookQuery).Result;
                string content = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = JsonConvert.DeserializeObject<ResponseMessage<BookListQuery>>(content);
                    responseData.Data.Keyword = bookQuery.Keyword;
                    responseData.Data.Page = bookQuery.Page;
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

            return responseData;
        }
    }
}
