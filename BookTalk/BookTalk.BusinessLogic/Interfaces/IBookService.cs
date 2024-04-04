using BookTalk.Shared.Aladin;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IBookService
{

    /// <summary>
    /// 도서 리스트 조회
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    public T GetBookData<T>(string url);

    /// <summary>
    /// 도서 리스트 요청 기본값 설정(신간 도서 or 베스트셀러)
    /// </summary>
    /// <param name="bookQuery"></param>
    /// <param name="type"></param>
    /// <param name="queryType"></param>
    public void SetBookList(BookListQuery bookQuery, string type, string queryType);

    /// <summary>
    /// 도서 리스트 요청 기본값 설정(도서 검색)
    /// </summary>
    /// <param name="bookQuery"></param>
    /// <param name="type"></param>
    public void SetBookSearch(BookListQuery bookQuery, string type);

    /// <summary>
    /// 도서 리스트 요청 기본값 설정(도서 상세)
    /// </summary>
    /// <param name="bookQuery"></param>
    /// <param name="type"></param>
    public void SetBookDetail(BookDetailQuery bookQuery, string type);

    /// <summary>
    /// 신간 도서 or 베스트 셀러 리스트를 가져오는 url 반환
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="key"></param>
    /// <param name="bookQuery"></param>
    /// <returns></returns>
    public string GetUrlForNewOrBestSellerBooks(string baseUrl, string key, BookListQuery bookQuery);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="key"></param>
    /// <param name="bookQuery"></param>
    /// <returns></returns>
    public string GetUrlForBookSearch(string baseUrl, string key, BookListQuery bookQuery);
}
