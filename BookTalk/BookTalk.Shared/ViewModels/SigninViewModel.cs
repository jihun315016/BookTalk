using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.ViewModels;

public class SigninViewModel
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID를 입력해주세요.")]
    public string Id { get; set; }

    [DisplayName("Password")]
    [Required(ErrorMessage = "비밀번호를 입력해주세요.")]
    public string Password { get; set; }
}
