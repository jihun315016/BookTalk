using BookTalk.Shared.Common;
using BookTalk.Shared.Exceptions;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Net;

namespace BookTalk.Client.Controllers;

public class AccountController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly string _baseApiUrl;

    public AccountController(IConfiguration configuration)
    {
        _configuration = configuration;
        _baseApiUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
    }

    [HttpGet]
    public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Signup(SignupViewModel viewModel)
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
                    Id = viewModel.Id,
                    Password = Utility.RunEncryption(viewModel.Password), // 암호화
                    Name = viewModel.Name
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
                return View(viewModel);
            }
        }
        catch (Exception ex)
        {
            // 실패 메세지
            ViewBag.ErrorMessage = Utility.GetMessage("msg04");
            responseData.InitializeResponseMessage(ex.Message, viewModel);
        }

        // 로그인 실패 시, 다시 기존 로그인 페이지로 이동
        return View(responseData.Data);
    }

    [HttpGet]
    public IActionResult Signin()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Signin(SigninViewModel viewModel)
    {
        ResponseMessage<Session> responseData = new ResponseMessage<Session>();
        var passwordHasher = new PasswordHasher<SigninViewModel>();
        User user = new User();
        string url;

        try
        {
            ModelState.Remove("FPassword");            

            if (ModelState.IsValid)
            {
                user = new User()
                {
                    Id = viewModel.Id,
                    Password = Utility.RunEncryption(viewModel.Password),
                    Name = ""
                };

                HttpClient client = new HttpClient();
                url = Utility.GetEndpointUrl(_baseApiUrl, "Account", "Signin");
                var response = client.PostAsJsonAsync(url, user).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                responseData = JsonConvert.DeserializeObject<ResponseMessage<Session>>(content);

                if (response.IsSuccessStatusCode) 
                {
                    string sessionId = _configuration.GetValue<string>("Session:id");
                    int sessionMinute = Convert.ToInt32(_configuration.GetValue<string>("Session:SessionMinute"));

                    // 클라이언트에 쿠키 설정
                    HttpContext.Response.Cookies.Append(
                        sessionId, 
                        responseData.Data.Id.ToString(),
                        new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = DateTimeOffset.Now.AddMinutes(sessionMinute)
                        }
                    );

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest && responseData.ErrorCode == Utility.GetUserStatusCodeNumber(UserStatusCode.ValidationError))
                    {
                        // 유효성 검사 실패                        
                        throw new UserValidationException(responseData.ValidationError.Message);
                    }
                    else
                    {
                        // 유효성 검사가 아닌 이유로 발생한 오류
                        responseData.ErrorCode = response.StatusCode.ToString();
                        throw new Exception(responseData.ErrorMessage);
                    }
                }
            }
        }
        catch (UserValidationException ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            responseData.InitializeResponseMessage(ex.Message, null);
        }
        catch (Exception ex)
        {
            // 실패 메세지
            ViewBag.ErrorMessage = Utility.GetMessage("msg01");
            responseData.InitializeResponseMessage(ex.Message, null);
        }

        // 로그인 실패 시, 다시 로그인 페이지로 이동
        return View(viewModel);
    }

    
    [HttpPost]
    public async Task<IActionResult> CheckValidUserAsync([FromBody] SigninViewModel viewModel)
    {
        ResponseMessage responseData = new ResponseMessage();
        User user = new User();
        string url;

        try
        {
            user = new User()
            {
                Id = viewModel.FId,
                Password = "",
                Name = viewModel.FName
            };

            HttpClient client = new HttpClient();
            url = Utility.GetEndpointUrl(_baseApiUrl, "Account", "CheckValidUser");
            var response = client.PostAsJsonAsync(url, user).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            responseData = JsonConvert.DeserializeObject<ResponseMessage>(content);

            if (response.IsSuccessStatusCode) 
            {
                return Ok(viewModel);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return StatusCode((int)response.StatusCode, new { message = Utility.GetMessage("msg06") });
                }
                else
                {
                    responseData.ErrorCode = response.StatusCode.ToString();
                    throw new Exception(responseData.ErrorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = Utility.GetMessage("msg01") });
        }
    }

    [HttpPost]
    public IActionResult ResetPassword([FromBody] SigninViewModel viewModel)
    {
        ResponseMessage responseData = new ResponseMessage();
        User user = new User();
        string url;

        try
        {
            ModelState.Remove("Id");
            ModelState.Remove("Password");

            if (ModelState.IsValid) 
            {
                user = new User()
                {
                    Id = viewModel.FId,
                    Password = Utility.RunEncryption(viewModel.FPassword),
                    Name = ""
                };

                HttpClient client = new HttpClient();
                url = Utility.GetEndpointUrl(_baseApiUrl, "Account", "ResetPassword");
                var response = client.PostAsJsonAsync(url, user).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                responseData = JsonConvert.DeserializeObject<ResponseMessage>(content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = Utility.GetMessage("msg07"), validMessage  = ""});
                }
                else
                {
                    responseData.ErrorCode = response.StatusCode.ToString();
                    throw new Exception(responseData.ErrorMessage);
                }
            }
            else
            {
                // 재설정할 비밀번호 유효설 검사 실패
                var message = ModelState.Values.SelectMany(m => m.Errors).Select(m => m.ErrorMessage).FirstOrDefault();
                return Json(new { message = "", validMessage = message});
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = Utility.GetMessage("msg01"), validMessage = "" });
        }
    }


    [HttpPost]
    public IActionResult Signout()
    {
        ResponseMessage responseData = new ResponseMessage();
        string url;
        string sessionKey;

        try
        {
            sessionKey = _configuration.GetValue<string>("Session:id");
            if (HttpContext.Request.Cookies.TryGetValue(sessionKey, out string sessionId))
            {
                // 쿠키 삭제
                HttpContext.Response.Cookies.Delete(sessionKey);

                Session session = new Session() { Id = sessionId };
                HttpClient client = new HttpClient();
                url = Utility.GetEndpointUrl(_baseApiUrl, "Account", "Signout");
                var response = client.PostAsJsonAsync(url, session).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                responseData = JsonConvert.DeserializeObject<ResponseMessage>(content);

                if (!response.IsSuccessStatusCode)
                {
                    responseData.ErrorCode = response.StatusCode.ToString();
                    throw new Exception(responseData.ErrorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = Utility.GetMessage("msg01") });
        }

        return Ok(new { message = Utility.GetMessage("msg08") });
    }
}
