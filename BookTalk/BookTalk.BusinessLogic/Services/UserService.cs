using BookTalk.Shared.Common;
using BookTalk.Shared.Contexts;
using BookTalk.Shared.Models;

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
}
