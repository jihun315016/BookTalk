using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Utility;

public class Utility
{
    public static string GetMessage(string code)
    {
        var resourceManager = Properties.Resources.ResourceManager;
        return resourceManager.GetString(code) ?? string.Empty;
    }
}
