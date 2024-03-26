using BookTalk.BusinessLogic.Interfaces;
using BookTalk.Shared.Contexts;
using BookTalk.Shared.Exceptions;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels;
using MongoDB.Driver;

namespace BookTalk.BusinessLogic.Services;

public class AccountService : IAccountService
{
    private readonly BookTalkDbContext _dbContext;
    private readonly MongoDBService _mongoDBService;

    public AccountService(BookTalkDbContext bookTalkDbContext, MongoDBService mongoDBService)
    {
        _dbContext = bookTalkDbContext;
        _mongoDBService = mongoDBService;
    }

    public void Signup(User user)
    {
        int iCount;
        iCount = _dbContext.Users.Count(u => u.Id == user.Id);
        if (iCount > 0)
        {
            throw new UserValidationException(nameof(SignupViewModel.Id), Utility.GetMessage("msg04"));
        }

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    public User Signin(User user)
    {
        User responseUser = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id && u.Password == user.Password);       
        if (responseUser == default(User))
        {
            // 존재하지 않는 계정
            throw new UserValidationException(Utility.GetMessage("msg05")); 
        }
        else
        {
            _mongoDBService.CreateOrUpdateSession(user.Id);
            return responseUser;
        }
    }
}
