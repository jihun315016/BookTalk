﻿using BookTalk.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.ViewModels.Account;

public class SignupViewModel
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID를 입력해주세요.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "ID는 5자 이상 20자 이하로 입력해주세요.")]
    public string Id { get; set; }

    [DisplayName("Password")]
    [Required(ErrorMessage = "비밀번호를 입력해주세요.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "비밀번호는 8자 이상 20자 이하로 입력해주세요.")]
    public string Password { get; set; }

    [DisplayName("Name")]
    [Required(ErrorMessage = "이름를 입력해주세요.")]
    public string Name { get; set; }
}
