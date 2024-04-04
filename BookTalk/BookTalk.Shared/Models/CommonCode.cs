using System;
using System.Collections.Generic;

namespace BookTalk.Shared.Models;

public partial class CommonCode
{
    public string Type { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Value { get; set; }
}
