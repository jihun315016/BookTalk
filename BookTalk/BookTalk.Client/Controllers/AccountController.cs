using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace BookTalk.Client.Controllers;

public class AccountController : Controller
{
    private readonly string _baseApiUrl;

    public AccountController(IConfiguration configuration)
    {
        _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
    }

    [HttpGet]
    public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Signup(SignupViewModel userViewModel)
    {
        ResponseMessage<SignupViewModel> responseData = new ResponseMessage<SignupViewModel>();
        var passwordHasher = new PasswordHasher<SignupViewModel>();
        User user = new User();
        string url;

        try
        {
            if (ModelState.IsValid)
            {
                user = new User()
                {
                    Id = userViewModel.Id,
                    Password = passwordHasher.HashPassword(userViewModel, userViewModel.Password), // 암호화
                    Name = userViewModel.Name
                };

                url = Utility.GetEndpointUrl(_baseApiUrl, "Account", "Signup");
                HttpClient client = new HttpClient();
                var response = client.PostAsJsonAsync(url, user).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                responseData = JsonConvert.DeserializeObject<ResponseMessage<SignupViewModel>>(content);

                if(response.IsSuccessStatusCode)
                {
                    // 회원가입 성공 메세지
                    TempData["SuccessMessage"] = Utility.GetMessage("msg03");

                    // 회원가입 성공 시 로그인 페이지로 이동
                    return RedirectToAction("Signin");
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest && responseData.ErrorCode == Utility.GetUserStatusCodeNumber(UserStatusCode.ValidationError))
                    {
                        // 추가 유효성 검사 실패
                        ModelState.AddModelError(responseData.ValidationError.Key, responseData.ValidationError.Message);
                    }
                    else
                    {
                        // 유효성 검사가 아닌 이유로 발생한 오류
                        responseData.ErrorCode = response.StatusCode.ToString();
                        throw new Exception(responseData.ErrorMessage);
                    }
                }
            }
            else
            {
                return View(userViewModel);
            }
        }
        catch (Exception ex)
        {
            // 회원가입 실패 메세지
            ViewBag.ErrorMessage = Utility.GetMessage("msg04");
            responseData.InitializeResponseMessage(ex.Message, userViewModel);
        }

        // 로그인 실패 시, 다시 회원가입 페이지로 이동
        return View(responseData.Data);
    }

    [HttpGet]
    public IActionResult Signin()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Signin(User user)
    {
        if (ModelState.IsValid)
        {
            // 로그인 처리
            return RedirectToAction("Index", "Home");            
        }

        // 로그인 실패 시, 다시 로그인 페이지로 이동
        return View();
    }
}
