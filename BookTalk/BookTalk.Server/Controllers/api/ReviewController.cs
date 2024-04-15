using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;
using BookTalk.Shared.Exceptions;
using BookTalk.Shared.Models;
using BookTalk.Shared.Utility;
using BookTalk.Shared.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;

namespace BookTalk.Server.Controllers.api;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    IConfiguration _configuration;
    private readonly ReviewService _reviewService;
    private readonly UserService _userService;
    private readonly BookService _bookService;
    private readonly CommentService _commentService;

    public ReviewController(IConfiguration configuration, ReviewService reviewService, UserService userService, BookService bookService, CommentService commentService)
    {
        _configuration = configuration;
        _reviewService = reviewService;
        _userService = userService;
        _bookService = bookService;
        _commentService = commentService;
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
                                 }).ToList().OrderByDescending(r => r.Id);

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


    [Route("Post")]
    [HttpPost]
    public IActionResult Post([FromBody] ReviewPostViewModel viewModel)
    {
        ResponseMessage responseData = new ResponseMessage();

        try
        {
            Review review = new Review()
            {
                Id = viewModel.Id == null ? 0 : (int)viewModel.Id,
                Title = viewModel.ReviewTitle,
                Isbn13 = viewModel.Isbn13,
                Isbn10 = viewModel.Isbn10,
                BookName = viewModel.BookTitle,
                UserId = _userService.GetUser(viewModel.SessionId).Id,
                Content = viewModel.Content,
                Rating = viewModel.Rating
            };

            _reviewService.CreateOrUpdate(review);
            return Ok();
        }
        catch (UserOverlapException ex)
        {
            responseData.ErrorCode = Utility.GetUserStatusCodeNumber(UserStatusCode.OverlapException);
            responseData.ErrorMessage = ex.Message;
            return StatusCode(StatusCodes.Status400BadRequest, responseData);
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
                Page = _commentService.SetCommentInfo(viewMocel.Id)
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
}
