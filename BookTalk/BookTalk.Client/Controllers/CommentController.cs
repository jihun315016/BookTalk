using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookTalk.Client.Controllers;

public class CommentController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ReviewController> _logger;
    private readonly string _baseApiUrl;

    public CommentController(IConfiguration configuration, ILogger<ReviewController> logger)
    {
        _configuration = configuration;
        _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        _logger = logger;
    }


    public IActionResult Index()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Post([FromBody] Comment comment)
    {
        ResponseMessage<Comment> responseData = new ResponseMessage<Comment>();
        CommentViewModel viewModel;
        string url;

        try
        {
            HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId);
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return Unauthorized(new { Message = Utility.GetMessage("msg08") });
            }

            viewModel = new CommentViewModel()
            {
                ReviewId = comment.ReviewId,
                CommentId = comment.CommentId,
                SessionId = sessionId,
                Content = comment.Content
            };

            //url = Utility.GetEndpointUrl(_baseApiUrl, "Comment", "Create");
            url = Utility.GetEndpointUrl(_baseApiUrl, "Comment", "Post");
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync(url, viewModel).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            responseData = JsonConvert.DeserializeObject<ResponseMessage<Comment>>(content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(responseData.Data == null ? new Comment() : responseData.Data);
            }
            else
            {
                responseData.ErrorCode = Convert.ToInt32(response.StatusCode).ToString();
                throw new Exception(responseData.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteError(_logger, ex);
            return StatusCode(Convert.ToInt32(responseData.ErrorCode), new { message = Utility.GetMessage("msg01") });
        }
    }
}
