using SharedKernel.Infrastructure.PayPal.Api;

namespace SharedKernel.Infrastructure.PayPal.Util;

/// <summary>Helper class that validates arguments.</summary>
internal class ArgumentValidator
{
    /// <summary>
    /// Helper method for validating an argument that will be used by this API in any requests.
    /// </summary>
    /// <param name="argument">The object to be validated.</param>
    /// <param name="argumentName">The name of the argument. This will be placed in the exception message for easy reference.</param>
    public static void Validate(object argument, string argumentName)
    {
        var str = argument as string;
        if (argument == null || str != null && string.IsNullOrEmpty(str))
            throw new ArgumentNullException(argumentName, argumentName + " cannot be null or empty");
    }

    /// <summary>
    /// Helper method for validating and setting up an APIContext object in preparation for it being used when sending an HTTP request.
    /// </summary>
    /// <param name="apiContext">APIContext used for API calls.</param>
    public static void ValidateAndSetupApiContext(APIContext apiContext)
    {
        Validate(apiContext, "APIContext");
        Validate(apiContext.AccessToken, "AccessToken");
        if (apiContext.HttpHeaders == null)
            apiContext.HttpHeaders = new Dictionary<string, string>();
        apiContext.HttpHeaders["Content-Type"] = "application/json";
        apiContext.SdkVersion = new SdkVersion();
    }
}