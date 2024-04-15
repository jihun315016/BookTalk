using BookTalk.Shared.ViewModels.Review;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BookTalk.Shared.ViewModels.User;

public class UserViewModel
{
    public string UserId { get; set; } 

    public string Name { get; set; } 

    public IEnumerable<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();

    public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

    [DisplayName("New password")]
    [Required(ErrorMessage = "변경할 비밀번호를 입력해주세요.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "비밀번호는 8자 이상 20자 이하로 입력해주세요.")]
    public string NewPassword { get; set; } = "";
}
