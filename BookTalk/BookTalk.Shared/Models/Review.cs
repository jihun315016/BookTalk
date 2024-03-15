﻿using System;
using System.Collections.Generic;

namespace BookTalk.Shared.Models;

public partial class Review
{
    public int Id { get; set; }

    public string Isbn { get; set; } = null!;

    public string? UserId { get; set; }

    public string Content { get; set; } = null!;

    public double Rating { get; set; }

    public DateOnly? CreateDate { get; set; }

    public int? LikeCount { get; set; }

    public int? DislikeCount { get; set; }

    public virtual User? User { get; set; }
}