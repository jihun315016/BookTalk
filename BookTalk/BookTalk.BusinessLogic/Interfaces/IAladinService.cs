using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IAladinService
{
    /// <summary>
    /// 도서 리스트 조회
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public AladinBookQuery GetBooks(string url);

    /// <summary>
    /// 신간 도서 or 베스트 셀러 리스트를 가져오는 url 반환
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="key"></param>
    /// <param name="aladinBookQuery"></param>
    /// <returns></returns>
    public string GetUrlForNewOrBestSellerBooks(string baseUrl, string key, AladinBookQuery aladinBookQuery);

    /// <summary>
    /// 검색한 조건에 맞는 도서를 가져오는 url 반환
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="key"></param>
    /// <param name="aladinBookQuery"></param>
    /// <returns></returns>
    public string GetUrlForBookSearch(string baseUrl, string key, AladinBookQuery aladinBookQuery);
}
