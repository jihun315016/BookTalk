using System;
using System.Collections.Generic;

namespace BookTalk.Shared.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
