using BookTalk.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.ViewModels;

public class UserViewModel
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID를 입력해주세요.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "ID는 5자 이상 20자 이하로 입력해주세요.")]
    public string InputId { get; set; }

    [DisplayName("Password")]
    [Required(ErrorMessage = "비밀번호를 입력해주세요.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "비밀번호는 8자 이상 20자 이하로 입력해주세요.")]
    public string InputPassword { get; set; }

    [DisplayName("Name")]
    [Required(ErrorMessage = "이름를 입력해주세요.")]
    public string InputName { get; set; }

    public User? User { get; set; }
}
