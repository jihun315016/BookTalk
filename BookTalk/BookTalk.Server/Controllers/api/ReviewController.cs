using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using BookTalk.Shared.Exceptions;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Numerics;

namespace BookTalk.Server.Controllers.api;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    IConfiguration _configuration;
    private readonly ReviewService _reviewService;
    private readonly UserService _userService;
    private readonly BookService _bookService;

    public ReviewController(IConfiguration configuration, ReviewService reviewService, UserService userService, BookService bookService)
    {
        _configuration = configuration;
        _reviewService = reviewService;
        _userService = userService;
        _bookService = bookService;
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
    public IActionResult Create([FromBody] ReviewPostViewModel viewMocel)
    {
        ResponseMessage responseData = new ResponseMessage();

        try
        {
            Review review = new Review()
            {
                Title = viewMocel.ReviewTitle,
                Isbn13 = viewMocel.Isbn13,
                Isbn10 = viewMocel.Isbn10,
                BookName = viewMocel.BookTitle,
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


    [Route("Read")]
    [HttpPost]
    public IActionResult Read([FromBody] ReviewViewModel viewMocel)
    {
        ResponseMessage<ReviewViewModel> responseData = new ResponseMessage<ReviewViewModel>();
        Review review;
        BookDetailQuery bookQuery;
        string key;
        string url;

        try
        {
            review = _reviewService.GetReview(viewMocel.Id);

            if (review == null)
            {
                throw new UserNoDataException();
            }

            bookQuery = new BookDetailQuery()
            {
                ItemIdType = string.IsNullOrWhiteSpace(review.Isbn13) ? "ISBN" : "ISBN13",
                ItemId = string.IsNullOrWhiteSpace(review.Isbn13) ? review.Isbn10 : review.Isbn13,
            };

            _bookService.SetBookDetail(bookQuery, _configuration["Aladin:DetailType"]);
            key = _configuration["Aladin:Key"];
            url = _bookService.GetUrlForOneBook(bookQuery.BaseUrl, key, bookQuery);
            bookQuery = _bookService.GetBookData<BookDetailQuery>(url);


            responseData.Data = new ReviewViewModel()
            {
                Id = viewMocel.Id,
                Title = review.Title,
                UserId = review.UserId,
                BookName = review.BookName,
                CreateDate = review.CreateDate,
                Rating = review.Rating,
                Content = review.Content,

                CurrentUserId = string.IsNullOrWhiteSpace(viewMocel.CurrentSessionId) ? "" : _userService.GetUser(viewMocel.CurrentSessionId).Id,

                CategoryName = _bookService.GetCategoryName(bookQuery.Item[0].CategoryId),
                Author = bookQuery.Item[0].Author,
                PubDate = bookQuery.Item[0].PubDate,
                Publisher = bookQuery.Item[0].Publisher,
                Cover = bookQuery.Item[0].Cover,
                Page = _reviewService.SetCommentInfo(viewMocel.Id)
            };

            return Ok(responseData);
        }
        catch (UserNoDataException ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.NoDataException);
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
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
    public IActionResult Delete([FromBody] ReviewViewModel viewMocel)
    {
        ResponseMessage<ReviewViewModel> responseData = new ResponseMessage<ReviewViewModel>();

        try
        {
            _reviewService.Delete(viewMocel.Id);
            return Ok(responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
    }


    [Route("CreateComment")]
    [HttpPost]
    public IActionResult CreateComment([FromBody] CommentViewModel viewMocel)
    {
        ResponseMessage<CommentViewModel> responseData = new ResponseMessage<CommentViewModel>();
        Comment comment;

        try
        {
            comment = _reviewService.CreateAndGetComment(new Comment()
            {
                ReviewId = viewMocel.ReviewId,
                UserId = _userService.GetUser(viewMocel.SessionId).Id,
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


    [Route("GetComments")]
    [HttpGet]
    public IActionResult GetComments(int reviewId, int page)
    {
        ResponseMessage<ReviewViewModel> responseData = new ResponseMessage<ReviewViewModel>();
        try
        {
            responseData.Data = _reviewService.GetComments(reviewId, page);
            return Ok(responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
    }

    [Route("DeleteComment")]
    [HttpDelete]
    public IActionResult DeleteComment([FromBody] CommentViewModel viewMocel)
    {
        ResponseMessage<CommentViewModel> responseData = new ResponseMessage<CommentViewModel>();

        try
        {
            _reviewService.DeleteComment(viewMocel.ReviewId, viewMocel.CommentId);
            return Ok(responseData);
        }
        catch (Exception ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.UndefinedError);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseData);
        }
    }


    [Route("PutComment")]
    [HttpPut]
    public IActionResult PutComment([FromBody] CommentViewModel viewMocel)
    {
        ResponseMessage<CommentViewModel> responseData = new ResponseMessage<CommentViewModel>();

        try
        {
            _reviewService.PutComment(viewMocel.ReviewId, viewMocel.CommentId, viewMocel.Content);
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
