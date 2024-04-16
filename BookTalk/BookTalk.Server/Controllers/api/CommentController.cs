using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Server.Controllers.api;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    IConfiguration _configuration;
    private readonly CommentService _commentService;
    private readonly UserService _userService;

    public CommentController(IConfiguration configuration, CommentService commentService, UserService userService)
    {
        _configuration = configuration;
        _commentService = commentService;
        _userService = userService;
    }

    [Route("Post")]
    [HttpPost]
    public IActionResult Post([FromBody] CommentViewModel viewMocel)
    {
        ResponseMessage<CommentViewModel> responseData = new ResponseMessage<CommentViewModel>();
        Comment comment;

        try
        {
            comment = _commentService.CreateOrUpdate(new Comment()
            {
                ReviewId = viewMocel.ReviewId,
                UserId = _userService.GetUser(viewMocel.SessionId).Id,
                CommentId = viewMocel.CommentId,
                Content = viewMocel.Content
            });

            responseData.Data = new CommentViewModel()
            {
                ReviewId = comment.ReviewId,
                CommentId = comment.CommentId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreateDate = comment.CreateDate
            };

            return Ok(responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
    }


    [Route("Get")]
    [HttpGet]
    public IActionResult Get(int reviewId, int page)
    {
        ResponseMessage<ReviewViewModel> responseData = new ResponseMessage<ReviewViewModel>();
        try
        {
            responseData.Data = _commentService.GetComments(reviewId, page);
            return Ok(responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
    }

    [Route("Delete")]
    [HttpDelete]
    public IActionResult Delete([FromBody] CommentViewModel viewMocel)
    {
        ResponseMessage<CommentViewModel> responseData = new ResponseMessage<CommentViewModel>();

        try
        {
            _commentService.Delete(viewMocel.ReviewId, viewMocel.CommentId);
            return Ok(responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
    }
}
