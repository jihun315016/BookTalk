using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Temps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.BusinessLogic.Services;

public class AladinService : IAladinService
{
    public async Task<ResponseMessage<AladinBookQuery>> GetBooks(string url)
    {
        ResponseMessage<AladinBookQuery> result = new ResponseMessage<AladinBookQuery>();
        HttpClient client = new HttpClient();

        try
        {
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                AladinBookQuery data = JsonConvert.DeserializeObject<AladinBookQuery>(content);
                result.Data = data;
            }
            else
            {
                result.ErrorCode = Convert.ToInt32(response.StatusCode);
            }
            return result;
        }
        catch (Exception ex)
        {
            result.ErrorCode = -1;
            result.ErrorMessage = ex.Message;
            return result;
        }
    }
}
