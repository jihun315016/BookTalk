﻿using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Server.Controllers.api;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("GetUser")]
    public IActionResult GetUser([FromBody] Session session)
    {
        ResponseMessage<User> responseData = new ResponseMessage<User>();

        try
        {
            responseData.Data = _userService.GetUser(session.Id);
            return Ok(responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
    }


    [HttpPost]
    [Route("Profile")]
    public IActionResult GetProfile([FromBody] Session session)
    {
        ResponseMessage<UserViewModel> responseData = new ResponseMessage<UserViewModel>();

        try
        {
            responseData.Data = _userService.GetProfile(session.Id);
            return Ok(responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
    }
}
