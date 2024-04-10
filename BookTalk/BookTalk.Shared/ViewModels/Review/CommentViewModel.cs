namespace BookTalk.Shared.ViewModels.Review;

public class CommentViewModel
{
    public int ReviewId { get; set; }

    public int CommentId { get; set; }

    public string SessionId { get; set; } = "";

    public string UserId { get; set; } = "";

    public string Content { get; set; } = "";

    public DateOnly? CreateDate { get; set; }
}
