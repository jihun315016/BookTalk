using System;
using System.Collections.Generic;

namespace BookTalk.Shared.Models;

public partial class Review
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Isbn10 { get; set; }

    public string? Isbn13 { get; set; }

    public string? BookName { get; set; }

    public string? UserId { get; set; }

    public string Content { get; set; } = null!;

    public double Rating { get; set; }

    public DateOnly? CreateDate { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User? User { get; set; }
}
