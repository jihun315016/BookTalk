using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Common;
using BookTalk.Shared.Exceptions;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Server.Controllers.api;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    [Route("Signup")]
    public IActionResult Signup([FromBody] User user)
    {
        ResponseMessage<User> responseData = new ResponseMessage<User>()
        {
            Data = user
        };

        try
        {
            _accountService.Signup(user);
            return Ok();
        }
        catch (UserValidationException ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.ValidationError);
            responseData.ValidationError.Message = ex.Message;
            return StatusCode(StatusCodes.Status400BadRequest, responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
    }

    [HttpPost]
    [Route("Signin")]
    public IActionResult Signin([FromBody] User user)
    {
        ResponseMessage<User> responseData = new ResponseMessage<User>()
        {
            Data = user
        };

        try
        {
            responseData.Data = _accountService.Signin(user);
            return Ok(responseData);
        }
        catch (UserValidationException ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.ValidationError);
            responseData.ValidationError.Message = ex.Message;
            return StatusCode(StatusCodes.Status400BadRequest, responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData) ;
        }
    }
}
