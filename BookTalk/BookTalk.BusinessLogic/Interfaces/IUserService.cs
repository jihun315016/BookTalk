using BookTalk.Shared.Models;
using BookTalk.Shared.ViewModels.User;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IUserService
{
    /// <summary>
    /// 세션 id를 통해 유저 조회
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public User GetUser(string sessionId);

    /// <summary>
    /// 사용자 프로필 정보 조회
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public UserViewModel GetProfile(string sessionId);
}
