using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookTalk.Client.ViewComponents.Home;

public class NavViewComponent : ViewComponent
{
    private readonly IConfiguration _configuration;
    private readonly string _baseApiUrl;

    public NavViewComponent(IConfiguration configuration)
    {
        _configuration = configuration;
        _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
    }

    public IViewComponentResult Invoke()
    {
        ResponseMessage<User> responseData = new ResponseMessage<User>();
        string url;

        try
        {
            if (HttpContext.Request.Cookies.TryGetValue(_configuration.GetValue<string>("Session:id"), out string sessionId))
            {
                Session session = new Session() { Id = sessionId };
                HttpClient client = new HttpClient();
                url = Utility.GetEndpointUrl(_baseApiUrl, "User", "GetUser");
                var response = client.PostAsJsonAsync(url, session).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                responseData = JsonConvert.DeserializeObject<ResponseMessage<User>>(content);

                if (!response.IsSuccessStatusCode)
                {
                    responseData.ErrorCode = response.StatusCode.ToString();
                    throw new Exception(responseData.ErrorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
            responseData.InitializeResponseMessage(ex.Message, null);
        }

        return View(responseData);
    }
}
