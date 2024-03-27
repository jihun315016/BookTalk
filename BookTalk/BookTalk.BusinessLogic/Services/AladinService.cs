using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Aladin;
using DnsClient;
using Newtonsoft.Json;

namespace BookTalk.BusinessLogic.Services;

public class AladinService : IAladinService
{
    public AladinBookQuery GetBooks(string url)
    {
        AladinBookQuery result = new AladinBookQuery();
        HttpClient client = new HttpClient();
        AladinBookQuery data = new AladinBookQuery();

        var response = client.GetAsync(url).Result;

        var content = response.Content.ReadAsStringAsync().Result;
        data = JsonConvert.DeserializeObject<AladinBookQuery>(content);

        for (int i = 0; i < data.Item.Count; i++)
        {
            // TODO : 개발 예정
            data.Item[i].CategoryName = "[임시] 카테고리 이름";
        }
        return data;        
    }


    public string GetUrlForNewOrBestSellerBooks(string baseUrl, string key, AladinBookQuery aladinBookQuery)
    {
        string query = $"ttbkey={key}&QueryType={aladinBookQuery.QueryType}&CategoryId={aladinBookQuery.CategoryId}" +
                $"&MaxResults={aladinBookQuery.MaxResult}&start={aladinBookQuery.Start}&SearchTarget={aladinBookQuery.SearchTarget}" +
                $"&cover={aladinBookQuery.Cover}&output={aladinBookQuery.Output}&Version=20131101";

        return baseUrl + query;        
    }


    public string GetUrlForBookSearch(string baseUrl, string key, AladinBookQuery aladinBookQuery)
    {
        string query = $"ttbkey={key}&Query={aladinBookQuery.Query}&QueryType={aladinBookQuery.QueryType}" +
            $"&MaxResults={aladinBookQuery.MaxResult}&start={aladinBookQuery.Start}&SearchTarget={aladinBookQuery.SearchTarget}&" +
            $"output={aladinBookQuery.Output}&Version=20131101";

        return baseUrl + query;
    }
}
