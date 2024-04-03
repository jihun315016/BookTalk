using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Server.Controllers.api;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _reviewService;
    private readonly UserService _userService;

    public ReviewController(ReviewService reviewService, UserService userService)
    {
        _reviewService = reviewService;
        _userService = userService;
    }

    [Route("Create")]
    [HttpPost]
    public IActionResult Create([FromBody] ReviewCreateViewModel viewMocel)
    {
        ResponseMessage responseData = new ResponseMessage();

        try
        {
            Review review = new Review()
            {
                Isbn13 = viewMocel.Isbn13,
                UserId = _userService.GetUser(viewMocel.SessionId).Id,
                Content = viewMocel.Content,
                Rating = viewMocel.Rating,
                CreateDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _reviewService.Create(review);
            return Ok();
        }
        catch (Exception ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
    }
}
