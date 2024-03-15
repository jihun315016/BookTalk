using BookTalk.Shared.Common;
using System.Security.Cryptography;
using System.Text;

namespace BookTalk.Shared.Utility;

public class Utility
{
    public static string GetMessage(string code)
    {
        var resourceManager = Properties.Resources.ResourceManager;
        return resourceManager.GetString(code) ?? string.Empty;
    }

    public static string GetEndpointUrl(string baseUrl, string controllerName, string actionName)
    {
        return $"{baseUrl}/api/{controllerName}/{actionName}";
    }

    public static string GetEndpointUrl(string baseUrl, string controllerName)
    {
        return $"{baseUrl}/api/{controllerName}";
    }

    public static string GetUserStatusCodeNumber(UserStatusCode statusCode)
    {
        return Convert.ToInt32(statusCode).ToString();
    }
}
