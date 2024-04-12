using BookTalk.Shared.Common;

namespace BookTalk.Shared.ViewModels.Review;

public class ReviewViewModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Author { get; set; } = "";

    public string UserId { get; set; } = "";

    public string BookName { get; set; }

    public DateOnly? CreateDate { get; set; }

    public DateTime? PubDate { get; set; } = default;

    public string Publisher { get; set; } = "";

    public double Rating { get; set; } = default;

    public string Content { get; set; } = "";

    public string Cover { get; set; } = "";

    public string CategoryName { get; set; } = "";

    public string CurrentSessionId { get; set; } = "";

    public string CurrentUserId { get; set; } = "";

    public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

    public Pagination Page { get; set; } = new Pagination();
}
