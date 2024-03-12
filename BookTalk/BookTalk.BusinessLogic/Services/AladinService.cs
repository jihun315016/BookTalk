using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using BookTalk.Shared.Utility;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BookTalk.BusinessLogic.Services;

public class AladinService : IAladinService
{
    public async Task<AladinBookQuery> GetBooks(string url)
    {
        AladinBookQuery result = new AladinBookQuery();
        HttpClient client = new HttpClient();
        AladinBookQuery data = new AladinBookQuery();

        try
        {
            var response = await client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();
            data = JsonConvert.DeserializeObject<AladinBookQuery>(content);

            for (int i = 0; i < data.Item.Count; i++)
            {
                // TODO : 개발 예정
                data.Item[i].CategoryName = "[임시] 카테고리 이름";
            }
            return data;
        }
        catch (Exception ex)
        {
            throw new Exception($"[{data.ErrorCode}] {data.ErrorMessage}");
        }
    }

    public string GetUrlBookList(string baseUrl, string key, AladinBookQuery aladinBookQuery)
    {
        string query = $"ttbkey={key}&QueryType={aladinBookQuery.QueryType}&CategoryId={aladinBookQuery.CategoryId}" +
                $"&MaxResults={aladinBookQuery.MaxResult}&start={aladinBookQuery.Start}&SearchTarget={aladinBookQuery.SearchTarget}" +
                $"&cover={aladinBookQuery.Cover}&output={aladinBookQuery.Output}&Version=20131101";

        return baseUrl + query;        
    }
}
