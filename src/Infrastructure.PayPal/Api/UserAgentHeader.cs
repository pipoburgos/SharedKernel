using System.Text;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>PayPal User-Agent Header implementation class</summary>
internal class UserAgentHeader
{
    /// <summary>Returns a PayPal specific User-Agent HTTP Header</summary>
    /// <returns>Dictionary containing User-Agent HTTP Header</returns>
    public static Dictionary<string, string> GetHeader()
    {
        return new Dictionary<string, string>
        {
            {
                "User-Agent",
                GetUserAgentHeader()
            },
        };
    }

    /// <summary>Creates the signature for the UserAgent header.</summary>
    /// <returns>A string containing the signature for the UserAgent header.</returns>
    private static string GetUserAgentHeader()
    {
        var stringBuilder = new StringBuilder("PayPalSDK/PayPal-NET-SDK " + BaseConstants.SdkVersion + " (");
        stringBuilder.Append(string.Join(";", FormatUserAgentParameter("lang", "DOTNET"),
            FormatUserAgentParameter("v", DotNetVersion), FormatUserAgentParameter("clr", DotNetClrVersion),
            FormatUserAgentParameter("bit", OperatingSystemBitness),
            FormatUserAgentParameter("os", OperatingSystemName)));
        stringBuilder.Append(')');
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Formats a parameter name and value to be used in the signature of a UserAgent header.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <returns>A formatted string containing both the parameter name and value.</returns>
    private static string FormatUserAgentParameter(string name, object value)
    {
        return $"{name}={value}";
    }

    /// <summary>
    /// Returns whether or not the operating system is 64-bit.
    /// </summary>
    /// <returns>True = 64-bit, False = 32-bit</returns>
    private static bool Is64Bit()
    {
        return Environment.Is64BitOperatingSystem;
    }

    /// <summary>Gets the bitness of the operating system.</summary>
    private static int OperatingSystemBitness => !Is64Bit() ? 32 : 64;

    /// <summary>Gets the name of the operating system.</summary>
    private static string OperatingSystemName => Environment.OSVersion.ToString();

    /// <summary>
    /// Gets the version of the current .NET common language runtime environment.
    /// </summary>
    private static string DotNetClrVersion => Environment.Version.ToString().Trim();

    /// <summary>Gets the version of the current .NET environment.</summary>
    private static string DotNetVersion => "4.5.1";
}