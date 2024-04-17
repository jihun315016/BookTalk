using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Common;
using BookTalk.Shared.Contexts;
using BookTalk.Shared.Models;
using BookTalk.Shared.ViewModels.Review;

namespace BookTalk.BusinessLogic.Services;

public class CommentService : ICommentService
{
    private readonly BookTalkDbContext _dbContext;

    public CommentService(BookTalkDbContext bookTalkDbContext)
    {
        _dbContext = bookTalkDbContext;
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


    public Comment CreateOrUpdate(Comment comment)
    {
        Comment newComment = _dbContext.Comments.FirstOrDefault(c => c.ReviewId == comment.ReviewId && c.CommentId == comment.CommentId);

        if (newComment == null) 
        {
            // Add
            List<Comment> list = _dbContext.Comments.Where(c => c.ReviewId == comment.ReviewId).ToList();
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
        else 
        {
            // Update
            newComment.Content = comment.Content;
            _dbContext.SaveChanges();
            return newComment;
        }
    }


    public void Delete(int reviewId, int commentId)
    {
        Comment comment = _dbContext.Comments.FirstOrDefault(c => c.ReviewId == reviewId && c.CommentId == commentId);

        if (comment != null)
        {
            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();
        }
    }


    private int GetCommentsCount(int reviewId)
    {
        return _dbContext.Comments.Where(c => c.ReviewId == reviewId).Count();
    }
}
