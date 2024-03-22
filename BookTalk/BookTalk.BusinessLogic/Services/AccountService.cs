using BookTalk.Shared.Common;
using BookTalk.Shared.Contexts;
using BookTalk.Shared.Exceptions;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookTalk.BusinessLogic.Services;

public class AccountService
{
    private readonly BookTalkDbContext _dbContext;

    public AccountService(BookTalkDbContext bookTalkDbContext)
    {
        _dbContext = bookTalkDbContext;
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
        User responseUser = _dbContext.Users.Find(user.Id);
        if (responseUser == null)
        {
            // 존재하지 않는 계정
            throw new UserValidationException(nameof(SignupViewModel.Id), Utility.GetMessage("msg04")); // 이거 메시지 만들어 고쳐야
        }
        else
        {
            return responseUser;
        }
    }
}
