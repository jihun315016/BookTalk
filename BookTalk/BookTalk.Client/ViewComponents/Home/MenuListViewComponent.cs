using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
//using BookTalk.Shared.Temps;
using BookTalk.Shared.Utility;

//using BookTalk.Shared.Temps;
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

                if (response.IsSuccessStatusCode) 
                {
                    responseData = JsonConvert.DeserializeObject<ResponseMessage<IEnumerable<Menu>>>(content);
                }
                else
                {
                    responseData.ErrorCode = response.StatusCode.ToString();
                    throw new Exception(responseData.DeveloperErrorMessage);
                }
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = Utility.GetMessage("msg02");
                responseData.DeveloperErrorMessage = ex.Message;
                responseData.Data = null;
            }
            finally
            {
                if (responseData == null)
                {
                    responseData= new ResponseMessage<IEnumerable<Menu>>();
                }
            }

            return View(responseData);
        }
    }
}
