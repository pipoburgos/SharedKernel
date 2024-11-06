using SharedKernel.Infrastructure.PayPal.Api;

namespace SharedKernel.Infrastructure.PayPal.Exceptions;

/// <summary>
/// Represents an error that occurred when making a call to PayPal's REST API.
/// </summary>
public class PaymentsException : HttpException
{
    /// <summary>
    /// Gets a <see cref="T:SharedKernel.Infrastructure.PayPal.Api.Error" /> JSON object containing the parsed details of the Payments error.
    /// </summary>
    public Error Details { get; private set; }

    /// <summary>
    /// Copy constructor that attempts to deserialize the response from the specified <seealso name="HttpException" />.
    /// </summary>
    /// <param name="ex">Originating <see cref="T:SharedKernel.Infrastructure.PayPal.HttpException" /> object that contains the details of the exception.</param>
    public PaymentsException(HttpException ex)
        : base(ex)
    {
        //if (string.IsNullOrEmpty(Response))
        //    return;

        //Details = JsonFormatter.ConvertFromJson<Error>(Response);
        //var stringBuilder = new StringBuilder();
        //stringBuilder.AppendLine();
        //stringBuilder.AppendLine("   Error:    " + Details.Name);
        //stringBuilder.AppendLine("   Message:  " + Details.Message);
        //stringBuilder.AppendLine("   URI:      " + Details.InformationLink);
        //stringBuilder.AppendLine("   Debug ID: " + Details.DebugId);
        //if (Details.Details != null)
        //    foreach (var detail in Details.Details)
        //        stringBuilder.AppendLine("   Details:  " + detail.Field + " -> " + detail.Issue);
        //LogMessage(stringBuilder.ToString());
    }

    /// <summary>
    /// Gets the prefix to use when logging the exception information.
    /// </summary>
    protected override string ExceptionMessagePrefix => "Payments Exception";
}