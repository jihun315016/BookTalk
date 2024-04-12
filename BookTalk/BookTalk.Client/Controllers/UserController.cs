using BookTalk.Shared.Common;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookTalk.Client.Controllers;

public class UserController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserController> _logger;
    private readonly string _baseApiUrl;

    public UserController(IConfiguration configuration, ILogger<UserController> logger)
    {
        _configuration = configuration;
        _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        _logger = logger;
    }

    public IActionResult Index()
    {
        ResponseMessage<UserViewModel> responseData = new ResponseMessage<UserViewModel>();
        Session session = new Session();
        string url;

        try
        {
            HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId);
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return RedirectToAction("Signin", "Account");
            }

            session.Id= sessionId;

            url = Utility.GetEndpointUrl(_baseApiUrl, "User", "Profile");

            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync(url, session).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                responseData = JsonConvert.DeserializeObject<ResponseMessage<UserViewModel>>(content);
            }
            else
            {
                responseData.ErrorCode = response.StatusCode.ToString();
                throw new Exception(responseData.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteError(_logger, ex);
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
        }

        return View(responseData.Data);
    }
}
