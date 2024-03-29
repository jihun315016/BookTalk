using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookTalk.Shared.ViewModels;

public class ReviewCreateViewModel
{
    [Required(ErrorMessage = "제목을 입력해주세요.")]
    public string ReviewTitle { get; set; }

    [Required(ErrorMessage = "평점을 입력해주세요.")]

    public int Rate { get; set; } 
    public IEnumerable<SelectListItem> Rates { get; set; }

    [Required(ErrorMessage = "도서를 입력해주세요.")]
    public string BookTitle { get; set; }

    [Required(ErrorMessage = "내용을 입력해주세요.")]
    public string Content { get; set; }
}
