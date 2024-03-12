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
}
