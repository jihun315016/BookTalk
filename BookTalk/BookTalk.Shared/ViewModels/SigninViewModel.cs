using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookTalk.Shared.ViewModels;

public class SigninViewModel
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID를 입력해주세요.")]
    public string Id { get; set; }

    [DisplayName("Password")]
    [Required(ErrorMessage = "비밀번호를 입력해주세요.")]
    public string Password { get; set; }

    [DisplayName("ID")]
    public string FId { get; set; } = "";

    [DisplayName("Password")]
    [Required(ErrorMessage = "비밀번호를 입력해주세요.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "비밀번호는 8자 이상 20자 이하로 입력해주세요.")]
    public string FPassword { get; set; }

    [DisplayName("Name")]
    public string FName { get; set; } = "";
}
