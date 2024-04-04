using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

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

    [Route("Search")]
    [HttpPost]
    public IActionResult Search([FromBody] ReviewIndexViewModel viewModel)
    {
        ResponseMessage<ReviewIndexViewModel> responseData = new ResponseMessage<ReviewIndexViewModel>();
        IEnumerable<Review> reviews;
        
        try
        {
            viewModel.SearchTypeCombo = _reviewService.GetReviewSearchType();

            reviews = _reviewService.Search(viewModel.QueryType, viewModel.Keyword);
            viewModel.Items = (from review in reviews
                                 select new ReviewViewModel()
                                 {
                                     Id = review.Id,
                                     Title = review.Title,
                                     UserId = review.UserId,
                                     BookName = review.BookName,
                                     CreateDate = review.CreateDate
                                 }).ToList();

            responseData.Data = viewModel;
            return Ok(responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
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
                Title = viewMocel.ReviewTitle,
                BookName = viewMocel.BookTitle,
                Isbn13 = viewMocel.Isbn13,
                UserId = _userService.GetUser(viewMocel.SessionId).Id,
                Content = viewMocel.Content,
                Rating = viewMocel.Rating
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
