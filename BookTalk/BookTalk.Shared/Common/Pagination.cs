namespace BookTalk.Shared.Common;

public class Pagination
{
    public int Page { get; set; }

    public int MinPage { get; set; }

    public int MaxPage { get; set; }

    public int PageUnit { get; set; }

    public int TotalResults { get; set; } = default;
}
