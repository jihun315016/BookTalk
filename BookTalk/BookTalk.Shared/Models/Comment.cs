using System;
using System.Collections.Generic;

namespace BookTalk.Shared.Models;

public partial class Comment
{
    public int ReviewId { get; set; }

    public int CommentId { get; set; }

    public string? UserId { get; set; }

    public string Content { get; set; } = null!;

    public int? ParentCommentId { get; set; }

    public DateOnly? CreateDate { get; set; }

    public virtual Review Review { get; set; } = null!;

    public virtual User? User { get; set; }
}
