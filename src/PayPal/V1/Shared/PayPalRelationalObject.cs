using PayPal.V1.Shared.Util;

namespace PayPal.V1.Shared;

/// <summary>
/// Represents a PayPal model object that will be returned from PayPal containing common resource data.
/// </summary>
public class PayPalRelationalObject : PayPalResource
{
    /// <summary>
    /// A list of HATEOAS (Hypermedia as the Engine of Application State) links.
    /// More information: https://developer.paypal.com/docs/api/#hateoas-links
    /// </summary>
    public List<Links>? Links { get; set; }

    /// <summary>
    /// Gets the HATEOAS link that matches the specified relation name.
    /// </summary>
    /// <param name="relationName">The name of the link relation.</param>
    /// <returns>A Links object containing the details of the HATEOAS link; null if not found.</returns>
    public Links? GetHateoasLink(string relationName)
    {
        return Links?.FirstOrDefault(l => l.Rel?.Equals(relationName) == true);
    }

    /// <summary>Gets the approval URL from a list of HATEOAS links.</summary>
    /// <param name="setUserActionParameter">If true, appends the 'useraction' URL query parameter.
    /// <para>For PayPal payments, this will set the approval button text on the PayPal site to "Pay Now".</para></param>
    /// <returns>The approval URL or an empty string if not found.</returns>
    public string GetApprovalUrl(bool setUserActionParameter = false)
    {
        var hateoasLink = GetHateoasLink("approval_url");
        return hateoasLink != null ? hateoasLink.Href + (setUserActionParameter ? "&useraction=commit" : string.Empty) : string.Empty;
    }

    /// <summary>
    /// Gets the resource token from an approval URL HATEOAS link, if found.
    /// </summary>
    /// <returns>A string containing the resource token associated with an approval URL.</returns>
    public string GetTokenFromApprovalUrl()
    {
        var approvalUrl = GetApprovalUrl();
        return (!string.IsNullOrEmpty(approvalUrl)
            ? SdkUtil.ParseQueryString(new Uri(approvalUrl).Query).Get("token")
            : string.Empty) ?? throw new InvalidOperationException();
    }
}


