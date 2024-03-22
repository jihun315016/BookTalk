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

    public static string RunEncryption(string str)
    {
        SHA256 sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
        StringBuilder sb = new StringBuilder();

        foreach (byte b in hash)
        {
            sb.AppendFormat("{0:x2}", b);
        }

        return sb.ToString();
    }
}
