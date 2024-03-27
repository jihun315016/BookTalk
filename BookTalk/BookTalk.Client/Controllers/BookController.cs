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
        public IActionResult SearchList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchListAsync(BookQuery bookQuery)
        {
            ResponseMessage<BookQuery> responseData = new ResponseMessage<BookQuery>();
            string url;

            try
            {
                url = Utility.GetEndpointUrl(_baseApiUrl, "Book", "SearchList");
                HttpClient client = new HttpClient();
                var response = await client.PostAsJsonAsync<BookQuery>(url, bookQuery);
                string content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    responseData = JsonConvert.DeserializeObject<ResponseMessage<BookQuery>>(content);
                }
                else
                {
                    responseData.ErrorCode = response.StatusCode.ToString();
                    throw new Exception(responseData.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
                responseData.ErrorMessage = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseData);
            }

            return View();
        }
    }
}
