using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookTalk.Client.ViewComponents.Home
{
    public class MenuListViewComponent : ViewComponent
    {
        private readonly string _baseApiUrl;

        public MenuListViewComponent(IConfiguration configuration)
        {
            _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }

        public IViewComponentResult Invoke()
        {
            ResponseMessage<IEnumerable<Menu>> responseData = new ResponseMessage<IEnumerable<Menu>>();
            string url;

            try
            {
                url = Utility.GetEndpointUrl(_baseApiUrl, "Menu", "GetAll");
                HttpClient client = new HttpClient();
                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                responseData = JsonConvert.DeserializeObject<ResponseMessage<IEnumerable<Menu>>>(content);

                if (!response.IsSuccessStatusCode) 
                {
                    responseData.ErrorCode = response.StatusCode.ToString();
                    throw new Exception(responseData.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = Utility.GetMessage("msg02");
                responseData.InitializeResponseMessage(ex.Message, null);
            }

            return View(responseData);
        }
    }
}
