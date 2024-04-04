using BookTalk.Shared.Models;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IMenuService
{
    /// <summary>
    /// 메뉴 리스트 조회
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Menu> GetAll();
}
