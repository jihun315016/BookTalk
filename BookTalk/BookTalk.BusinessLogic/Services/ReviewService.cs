using Azure;
using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Common;
using BookTalk.Shared.Contexts;
using BookTalk.Shared.Models;
using BookTalk.Shared.ViewModels.Review;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;

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

    public ReviewViewModel GetComments(int reviewId, int page = 1)
    {
        ReviewViewModel viewModel = new ReviewViewModel();
        int unit = Convert.ToInt32(_dbContext.CommonCodes.Where(c => c.Type == "Comment-Info").FirstOrDefault(c => c.Code == "PageUnit").Value);
        IEnumerable<Comment> list = _dbContext.Comments.Where(c => c.ReviewId == reviewId).ToList();

        viewModel.Page = SetCommentInfo(reviewId, page);
        viewModel.Comments = (from item in list
                              select new CommentViewModel()
                              {
                                  ReviewId = item.ReviewId,
                                  CommentId = item.CommentId,
                                  UserId = item.UserId,
                                  Content = item.Content,
                                  CreateDate = item.CreateDate
                              }).ToList().OrderByDescending(l => l.CommentId).Skip(unit * (page - 1)).Take(unit).ToList();
        return viewModel;
    }


    private int GetCommentsCount(int reviewId)
    {
        return _dbContext.Comments.Where(c => c.ReviewId == reviewId).Count();
    }


    public Pagination SetCommentInfo(int reviewId, int currentPage = 1)
    {
        IEnumerable<CommonCode> commonCodes = _dbContext.CommonCodes.Where(c => c.Type == "Comment-Info").ToList();
        Pagination page = new Pagination();
        page.PageUnit = Convert.ToInt32(commonCodes.FirstOrDefault(c => c.Code == "PageUnit").Value);
        page.TotalResults = GetCommentsCount(reviewId);
        page.Page = currentPage;
        page.MinPage = 1;
        page.MaxPage = (int)Math.Ceiling((decimal)page.TotalResults / 5);
        return page;
    }


    public Comment CreateAndGetComment(Comment comment)
    {
        List<Comment> list = _dbContext.Comments.Where(c => c.ReviewId == comment.ReviewId).ToList();
        
        // CommentId 가져와서 넣어주기
        if (list.Count > 0) 
        {
            comment.CommentId = (from item in list select item.CommentId).ToList().Max() + 1;
        }
        else
        {
            comment.CommentId = 1;
        }

        _dbContext.Comments.Add(comment);
        _dbContext.SaveChanges();

        return _dbContext.Comments.FirstOrDefault(c => c.ReviewId == comment.ReviewId && c.CommentId == comment.CommentId);
    }
}
