using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookTalk.Shared.ViewModels.Review;

public class ReviewPostViewModel
{
    #region POST
    public int? Id { get; set; }

    public string? SessionId { get; set; }

    [DisplayName("리뷰 제목")]
    [Required(ErrorMessage = "리뷰 제목을 입력해주세요.")]
    public string ReviewTitle { get; set; }

    [DisplayName("평점")]
    [Required(ErrorMessage = "평점을 입력해주세요.")]
    public int Rating { get; set; }

    public IEnumerable<SelectListItem>? Rates { get; set; }

    [DisplayName("도서")]
    [Required(ErrorMessage = "도서를 입력해주세요.")]
    public string BookTitle { get; set; }

    [DisplayName("저자")]
    public string Author { get; set; }

    [Required(ErrorMessage = "내용을 입력해주세요.")]
    public string Content { get; set; }

    public string? Isbn10 { get; set; }

    public string? Isbn13 { get; set; }

    public string? Cover { get; set; }
    #endregion

    #region 도서 검색 팝업
    [DisplayName("도서")]
    [JsonProperty("keyword")]
    public string Keyword { get; set; } = "";

    [JsonProperty("page")]
    public int Page { get; set; } = 1;
    #endregion
}
