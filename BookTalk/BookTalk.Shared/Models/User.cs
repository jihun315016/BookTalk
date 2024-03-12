using System;
using System.Collections.Generic;

namespace BookTalk.Shared.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ProfileImagePath { get; set; }

    public int? Following { get; set; }

    public int? Follower { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<User> Followers { get; set; } = new List<User>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
