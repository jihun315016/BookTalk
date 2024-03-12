using System;
using System.Collections.Generic;

namespace BookTalk.Shared.Models;

public partial class Menu
{
    public int Id { get; set; }

    public int? Level { get; set; }

    public string MenuName { get; set; } = null!;

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public int? ParentMenuId { get; set; }
}
