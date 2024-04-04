using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Contexts;
using BookTalk.Shared.Models;
using DnsClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace BookTalk.BusinessLogic.Services;

public class BookService : IBookService
{
    private readonly BookTalkDbContext _dbContext;

    public BookService(BookTalkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public BookListQuery GetBooks(string url)
    {
        HttpClient client = new HttpClient();
        BookListQuery data = new BookListQuery();

        var response = client.GetAsync(url).Result;

        var content = response.Content.ReadAsStringAsync().Result;
        data = JsonConvert.DeserializeObject<BookListQuery>(content);
        return data;
    }


    public void SetBookList(BookListQuery bookQuery, string type, string queryType)
    {
        IEnumerable<CommonCode> commonCodes = _dbContext.CommonCodes.Where(c => c.Type == type).ToList();
        bookQuery.BaseUrl = commonCodes.FirstOrDefault(c => c.Code == "BaseUrl").Value;
        bookQuery.Output = commonCodes.FirstOrDefault(c => c.Code == "Output").Value;
        bookQuery.MaxResult = Convert.ToInt32(commonCodes.FirstOrDefault(c => c.Code == "MaxResult").Value);
        bookQuery.SearchTarget = commonCodes.FirstOrDefault(c => c.Code == "SearchTarget").Value;
        bookQuery.QueryType = queryType;
        bookQuery.Item = new List<Book>();
    }


    public void SetBookSearch(BookListQuery bookQuery, string type)
    {
        IEnumerable<CommonCode> commonCodes = _dbContext.CommonCodes.Where(c => c.Type == type).ToList();
        bookQuery.BaseUrl = commonCodes.FirstOrDefault(c => c.Code == "BaseUrl").Value;
        bookQuery.Output = commonCodes.FirstOrDefault(c => c.Code == "Output").Value;
        bookQuery.MaxResult = Convert.ToInt32(commonCodes.FirstOrDefault(c => c.Code == "MaxResult").Value);
        bookQuery.SearchTarget = commonCodes.FirstOrDefault(c => c.Code == "SearchTarget").Value;
        bookQuery.QueryType = commonCodes.FirstOrDefault(c => c.Code == "QueryType").Value;
        bookQuery.MinPage = Convert.ToInt32(commonCodes.FirstOrDefault(c => c.Code == "MinPage").Value);
        bookQuery.MaxPage = Convert.ToInt32(commonCodes.FirstOrDefault(c => c.Code == "MaxPage").Value);
        bookQuery.Start = bookQuery.MaxResult * (bookQuery.Page - 1) + 1;
        bookQuery.Item = new List<Book>();
    }


    public void SetBookDetail(BookDetailQuery bookQuery, string type)
    {
        IEnumerable<CommonCode> commonCodes = _dbContext.CommonCodes.Where(c => c.Type == type).ToList();
        bookQuery.BaseUrl = commonCodes.FirstOrDefault(c => c.Code == "BaseUrl").Value;
        bookQuery.Output = commonCodes.FirstOrDefault(c => c.Code == "Output").Value;
        bookQuery.OptResult = commonCodes.FirstOrDefault(c => c.Code == "OptResult").Value;
    }


    public string GetUrlForNewOrBestSellerBooks(string baseUrl, string key, BookListQuery bookQuery)
    {
        string query = $"ttbkey={key}&QueryType={bookQuery.QueryType}&CategoryId={bookQuery.CategoryId}" +
                $"&MaxResults={bookQuery.MaxResult}&start={bookQuery.Start}&SearchTarget={bookQuery.SearchTarget}" +
                $"&cover={bookQuery.Cover}&output={bookQuery.Output}&Version=20131101";

        return baseUrl + query;        
    }


    public string GetUrlForBookSearch(string baseUrl, string key, BookListQuery bookQuery)
    {
        string query = $"ttbkey={key}&Query={bookQuery.Query}&QueryType={bookQuery.QueryType}" +
            $"&MaxResults={bookQuery.MaxResult}&start={bookQuery.Start}&SearchTarget={bookQuery.SearchTarget}&" +
            $"output={bookQuery.Output}&Version=20131101";

        return baseUrl + query;
    }


    public string GetUrlForOneBook(string baseUrl, string key, BookDetailQuery bookQuery)
    {
        string query = $"ttbkey={key}&itemIdType={bookQuery.ItemIdType}&ItemId={bookQuery.ItemId}&output={bookQuery.Output}" +
            $"&Version=20131101&OptResult={bookQuery.OptResult}";

        return baseUrl + query;
    }
}
