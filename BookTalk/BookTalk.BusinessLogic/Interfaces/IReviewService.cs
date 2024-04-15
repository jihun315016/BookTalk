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
    /// 리뷰 작성 글 생성 또는 수정
    /// </summary>
    /// <param name="review"></param>
    public void CreateOrUpdate(Review review);

    /// <summary>
    /// 리뷰 글 가져오기
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Review GetReview(int id);


    /// <summary>
    /// 리뷰 글 삭제
    /// </summary>
    /// <param name="reviewId"></param>
    public void Delete(int reviewId);
}
