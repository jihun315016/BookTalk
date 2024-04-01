using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using BookTalk.Shared.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            ResponseMessage<BookQuery> responseData = new ResponseMessage<BookQuery>();
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

            return View(responseData.Data);
        }


        [HttpGet]
        public async Task<IActionResult> SearchListJson(string? keyword, int page = 1)
        {
            ResponseMessage<BookQuery> responseData = new ResponseMessage<BookQuery>();
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


        private ResponseMessage<BookQuery> GetSearchBooks(string keyword, int page)
        {
            ResponseMessage<BookQuery> responseData = new ResponseMessage<BookQuery>();
            string url;

            try
            {
                BookQuery bookQuery = new BookQuery()
                {
                    Query = string.IsNullOrWhiteSpace(keyword) ? "" : keyword,
                    Keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword,
                    Page = page
                };

                url = Utility.GetEndpointUrl(_baseApiUrl, "Book", "SearchList");
                HttpClient client = new HttpClient();
                var response = client.PostAsJsonAsync<BookQuery>(url, bookQuery).Result;
                string content = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    responseData = JsonConvert.DeserializeObject<ResponseMessage<BookQuery>>(content);
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
