using BookTalk.Shared.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IReviewService
{
    /// <summary>
    /// 리뷰 글 검색 조건 조회
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SelectListItem> GetReviewSearchType();

    /// <summary>
    /// 리뷰 작성 글 조회
    /// </summary>
    /// <param name="title"></param>
    /// <param name="bookName"></param>
    /// <returns></returns>
    public IEnumerable<Review> Search(string title, string bookName);

    /// <summary>
    /// 리뷰 작성 글 생성
    /// </summary>
    /// <param name="review"></param>
    public void Create(Review review);
}
