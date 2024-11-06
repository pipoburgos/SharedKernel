namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>Class for inspecting the ID and version of this SDK.</summary>
public class SdkVersion
{
    /// <summary>SDK ID used in User-Agent HTTP header</summary>
    /// <returns>SDK ID</returns>
    public static string GetSdkId()
    {
        return "PayPal-NET-SDK";
    }

    /// <summary>SDK Version used in User-Agent HTTP header</summary>
    /// <returns>SDK Version</returns>
    public static string GetSdkVersion()
    {
        return BaseConstants.SdkVersion;
    }
}