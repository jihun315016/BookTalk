using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Aladin;
using DnsClient;
using Newtonsoft.Json;

namespace BookTalk.BusinessLogic.Services;

public class BookService : IBookService
{
    public BookQuery GetBooks(string url)
    {
        BookQuery result = new BookQuery();
        HttpClient client = new HttpClient();
        BookQuery data = new BookQuery();

        var response = client.GetAsync(url).Result;

        var content = response.Content.ReadAsStringAsync().Result;
        data = JsonConvert.DeserializeObject<BookQuery>(content);

        for (int i = 0; i < data.Item.Count; i++)
        {
            // TODO : 개발 예정
            data.Item[i].CategoryName = "[임시] 카테고리 이름";
        }
        return data;        
    }


    public string GetUrlForNewOrBestSellerBooks(string baseUrl, string key, BookQuery bookQuery)
    {
        string query = $"ttbkey={key}&QueryType={bookQuery.QueryType}&CategoryId={bookQuery.CategoryId}" +
                $"&MaxResults={bookQuery.MaxResult}&start={bookQuery.Start}&SearchTarget={bookQuery.SearchTarget}" +
                $"&cover={bookQuery.Cover}&output={bookQuery.Output}&Version=20131101";

        return baseUrl + query;        
    }


    public string GetUrlForBookSearch(string baseUrl, string key, BookQuery bookQuery)
    {
        string query = $"ttbkey={key}&Query={bookQuery.Query}&QueryType={bookQuery.QueryType}" +
            $"&MaxResults={bookQuery.MaxResult}&start={bookQuery.Start}&SearchTarget={bookQuery.SearchTarget}&" +
            $"output={bookQuery.Output}&Version=20131101";

        return baseUrl + query;
    }
}
