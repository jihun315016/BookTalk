using BookTalk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IAccountService
{
    /// <summary>
    /// 회원가입
    /// </summary>
    /// <param name="user"></param>
    public void Signup(User user);

    /// <summary>
    /// 로그인
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public User Signin(User user);

    /// <summary>
    /// 비밀번호 재설정 처리 전 사용자 유효성 검사
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public bool CheckValidUser(User user);

    /// <summary>
    /// 비밀번호 재설정
    /// </summary>
    /// <param name="user"></param>
    public void ResetPassword(User user);
}
