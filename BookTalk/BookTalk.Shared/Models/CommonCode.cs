using System;
using System.Collections.Generic;

namespace BookTalk.Shared.Models;

public partial class CommonCode
{
    public int Id { get; set; }

    public string Tp { get; set; } = null!;

    public string? Code { get; set; }

    public string? Value { get; set; }
}
