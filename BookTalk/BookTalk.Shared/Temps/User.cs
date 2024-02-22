using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Temps;

public class User
{
    public string UserId { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public string ProfileImagePath { get; set; }

    public int FollowerCount { get; set; }
}
