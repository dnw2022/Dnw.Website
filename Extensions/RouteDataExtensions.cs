namespace Dnw.Website.Extensions;

public static class RouteDataExtensions
{
    public static string GetRequiredString(this RouteData routeData, string keyName)
    {
        return !routeData.Values.TryGetValue(keyName, out var value) ? string.Empty : value?.ToString() ?? string.Empty;
    }
}