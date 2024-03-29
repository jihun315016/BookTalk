using BookTalk.Shared.Models;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IUserService
{
    public User GetUser(string sessionId);
}
