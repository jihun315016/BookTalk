using BookTalk.Shared.Temps;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Client.ViewComponents.Home
{
    public class MenuListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            IEnumerable<Menu> menuList = new List<Menu>()
            {
                new Menu {ID = 1, Level = 0, ControllerName = "", ActionName = "", MenuName = "사용자", ParentMenuID = 0},
                new Menu {ID = 2, Level = 1, ControllerName = "User", ActionName = "Index", MenuName = "프로필", ParentMenuID = 1},
                new Menu {ID = 3, Level = 1, ControllerName = "User", ActionName = "Edit", MenuName = "정보 수정", ParentMenuID = 1},
                new Menu {ID = 4, Level = 1, ControllerName = "User", ActionName = "Delete ", MenuName = "회원 탈퇴", ParentMenuID = 1},
                new Menu {ID = 5, Level = 0, ControllerName = "", ActionName = "", MenuName = "게시판", ParentMenuID = 0},
                new Menu {ID = 6, Level = 1, ControllerName = "Review", ActionName = "Index", MenuName = "도서 리뷰", ParentMenuID = 5},
                new Menu {ID = 7, Level = 1, ControllerName = "Friendship", ActionName = "Index", MenuName = "커뮤니티", ParentMenuID = 5}
            };

            return View(menuList);
        }
    }
}
