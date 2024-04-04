using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Contexts;
using BookTalk.Shared.Models;
using Newtonsoft.Json;

namespace BookTalk.BusinessLogic.Services;

public class BookService : IBookService
{
    private readonly BookTalkDbContext _dbContext;

    public BookService(BookTalkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public T GetBookData<T>(string url)
    {
        HttpClient client = new HttpClient();
        T data = Activator.CreateInstance<T>();

        var response = client.GetAsync(url).Result;

        var content = response.Content.ReadAsStringAsync().Result;
        data = JsonConvert.DeserializeObject<T>(content);
        return data;
    }


    public string GetCategoryName(string categoryId)
    {
        return _dbContext.BookCategories.FirstOrDefault(c => ((int)c.CategoryId).ToString() == categoryId).CategoryName;
    }


    public double GetRating(string? isbn13, string? isbn10)
    {
        List<double> list = _dbContext.Reviews.Where(r => r.Isbn13 == isbn13).Select(r => r.Rating).ToList();
        if (list.Count == 0) // isbn13이 없으면 isbn 확인
        {
            list = _dbContext.Reviews.Where(r => r.Isbn10 == isbn10).Select(r => r.Rating).ToList();
        }

        // 리뷰 작성된 리스트가 하나도 없으면 0 반환
        if (list.Count == 0)
        {
            return 0;
        }
        else
        {
            return list.Average();
        }
    }


    public void SetBookList(BookListQuery bookQuery, string type, string queryType)
    {
        IEnumerable<CommonCode> commonCodes = _dbContext.CommonCodes.Where(c => c.Type == type).ToList();
        bookQuery.BaseUrl = commonCodes.FirstOrDefault(c => c.Code == "BaseUrl").Value;
        bookQuery.Output = commonCodes.FirstOrDefault(c => c.Code == "Output").Value;
        bookQuery.MaxResult = Convert.ToInt32(commonCodes.FirstOrDefault(c => c.Code == "MaxResult").Value);
        bookQuery.SearchTarget = commonCodes.FirstOrDefault(c => c.Code == "SearchTarget").Value;
        bookQuery.Cover = commonCodes.FirstOrDefault(c => c.Code == "Cover").Value;
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
        bookQuery.Cover = commonCodes.FirstOrDefault(c => c.Code == "Cover").Value;
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
        bookQuery.Cover = commonCodes.FirstOrDefault(c => c.Code == "Cover").Value;
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
            $"&Cover={bookQuery.Cover}&Version=20131101&OptResult={bookQuery.OptResult}";

        return baseUrl + query;
    }
}
