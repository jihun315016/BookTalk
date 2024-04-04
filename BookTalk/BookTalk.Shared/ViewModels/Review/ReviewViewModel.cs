namespace BookTalk.Shared.ViewModels.Review;

public class ReviewViewModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string UserId { get; set; }

    public string BookName { get; set; }

    public DateOnly? CreateDate { get; set; }

    public double Rating { get; set; } = default;

    public string Content { get; set; } = "";
}
