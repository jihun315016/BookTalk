using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Contexts;
using BookTalk.Shared.Models;
using BookTalk.Shared.ViewModels.Review;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookTalk.BusinessLogic.Services;

public class ReviewService : IReviewService
{
    private readonly BookTalkDbContext _dbContext;

    public ReviewService(BookTalkDbContext bookTalkDbContext)
    {
        _dbContext = bookTalkDbContext;
    }

    public IEnumerable<SelectListItem> GetReviewSearchType()
    {
        List<SelectListItem> list = new List<SelectListItem>();
        IEnumerable<CommonCode> commonCodes = _dbContext.CommonCodes.Where(c => c.Type == "Review-Search-Type").OrderBy(c => c.Code).ToList();
        foreach (var item in commonCodes)
        {
            list.Add(new SelectListItem { Value = item.Code, Text = item.Value, Selected = item.Code == "1" });
        }
        return list;
    }

    public IEnumerable<Review> Search(string queryType, string keyword)
    {
        switch (queryType)
        {
            case "1": // 조건 없음
                return _dbContext.Reviews.OrderByDescending(r => r.CreateDate).ToList();
            case "2": // 글 제목 검색
                return _dbContext.Reviews.Where(r => r.Title.Contains(keyword)).OrderByDescending(r => r.CreateDate).ToList();
            case "3": // 도서명 검색
                return _dbContext.Reviews.Where(r => r.BookName.Contains(keyword)).OrderByDescending(r => r.CreateDate).ToList();
            default: // 처음 로드될 때
                return _dbContext.Reviews.OrderByDescending(r => r.CreateDate).ToList();
        }
    }    

    public void Create(Review review)
    {
        _dbContext.Reviews.Add(review);
        _dbContext.SaveChanges();
    }

    public Review GetReview(int id)
    {
        return _dbContext.Reviews.FirstOrDefault(r => r.Id == id);
    }
}
