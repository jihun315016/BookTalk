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
            throw new UserValidationException(nameof(UserViewModel.InputId), Utility.GetMessage("msg05"));
        }

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }
}
