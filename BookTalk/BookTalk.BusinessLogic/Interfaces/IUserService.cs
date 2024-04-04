using BookTalk.Shared.Models;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IUserService
{
    /// <summary>
    /// 세션 id를 통해 유저 조회
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public User GetUser(string sessionId);
}
