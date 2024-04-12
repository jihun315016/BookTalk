using BookTalk.Shared.Common;
using BookTalk.Shared.Contexts;
using BookTalk.Shared.Models;
using BookTalk.Shared.ViewModels.Review;
using BookTalk.Shared.ViewModels.User;

namespace BookTalk.BusinessLogic.Services;

public class UserService
{
    private readonly BookTalkDbContext _dbContext;
    private readonly MongoDBService _mongoDBService;

    public UserService(BookTalkDbContext bookTalkDbContext, MongoDBService mongoDBService)
    {
        _dbContext = bookTalkDbContext;
        _mongoDBService = mongoDBService;
    }

    public User GetUser(string sessionId)
    {
        Session session = _mongoDBService.GetSession(sessionId);
        User user = _dbContext.Users.FirstOrDefault(u => u.Id == session.UserId);
        return user;
    }

    public UserViewModel GetProfile(string sessionId)
    {
        Session session = _mongoDBService.GetSession(sessionId);
        User user = _dbContext.Users.FirstOrDefault(u => u.Id == session.UserId);
        IEnumerable<Review> reviews = _dbContext.Reviews.Where(r => r.UserId == user.Id).OrderByDescending(r => r.Id).Take(10).ToList();
        IEnumerable<Comment> comments = _dbContext.Comments.Where(c => c.UserId == user.Id).OrderByDescending(c => c.ReviewId).Take(10).ToList();

        UserViewModel viewModel = new UserViewModel()
        {
            UserId = user.Id,
            Name = user.Name,
            Reviews = from review in reviews
                      select new ReviewViewModel()
                      {
                          Id = review.Id,
                          Title = review.Title,
                          BookName = review.BookName,
                          CreateDate = review.CreateDate
                      },
            Comments = from comment in _dbContext.Comments
                       join review in _dbContext.Reviews on comment.ReviewId equals review.Id
                       select new CommentViewModel
                       {
                           ReviewId = comment.ReviewId,
                           ReviewTitle = review.Title,
                           Content = comment.Content,
                           CreateDate = comment.CreateDate
                       }
        };

        return viewModel;
    }
}
