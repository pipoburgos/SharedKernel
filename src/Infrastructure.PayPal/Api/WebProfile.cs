using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Payment web experience profile.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class WebProfile : PayPalResource
{
    /// <summary>The ID of the web experience profile.</summary>
    public string Id { get; set; }

    /// <summary>
    /// The web experience profile name. Unique for a specified merchant's profiles.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Indicates whether the profile persists for three hours or permanently. Set to `false` to persist the profile permanently. Set to `true` to persist the profile for three hours.
    /// </summary>
    public bool? Temporary { get; set; }

    /// <summary>Parameters for flow configuration.</summary>
    public FlowConfig FlowConfig { get; set; }

    /// <summary>Parameters for input fields customization.</summary>
    public InputFields InputFields { get; set; }

    /// <summary>Parameters for style and presentation.</summary>
    public Presentation Presentation { get; set; }

    /// <summary>
    /// Creates a web experience profile. Pass the profile name and details in the JSON request body.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>CreateProfileResponse</returns>
    public CreateProfileResponse Create(APIContext apiContext)
    {
        return Create(apiContext, this);
    }

    /// <summary>
    /// Create a web experience profile by passing the name of the profile and other profile details in the request JSON to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="webProfile">WebProfile object to be created as a PayPal resource.</param>
    /// <returns>CreateProfileResponse</returns>
    public static CreateProfileResponse Create(APIContext apiContext, WebProfile webProfile)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        const string resource = "v1/payment-experience/web-profiles";
        return ConfigureAndExecute<CreateProfileResponse>(apiContext, HttpMethod.Post, resource, webProfile);
    }

    /// <summary>
    /// Updates a web experience profile. Pass the ID of the profile to the request URI and pass the profile details in the JSON request body. If your request omits any profile detail fields, the operation removes the previously set values for those fields.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    public void Update(APIContext apiContext)
    {
        Update(apiContext, this);
    }

    /// <summary>
    /// Update a web experience profile by passing the ID of the profile to the request URI. In addition, pass the profile details in the request JSON. If your request does not include values for all profile detail fields, the previously set values for the omitted fields are removed by this operation.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="profile">WebProfile resource to update.</param>
    public static void Update(APIContext apiContext, WebProfile profile)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(profile, nameof(profile));
        var resource = SdkUtil.FormatUriPath("v1/payment-experience/web-profiles/{0}", [
            profile.Id,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Put, resource, profile);
    }

    /// <summary>
    /// Partially-updates a web experience profile. Pass the profile ID to the request URI. Pass a patch object with the operation, path of the profile location to update, and, if needed, a new value to complete the operation in the JSON request body.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="patchRequest">PatchRequest</param>
    public void PartialUpdate(APIContext apiContext, PatchRequest patchRequest)
    {
        PartialUpdate(apiContext, Id, patchRequest);
    }

    /// <summary>
    /// Partially update an existing web experience profile by passing the ID of the profile to the request URI. In addition, pass a patch object in the request JSON that specifies the operation to perform, path of the profile location to update, and a new value if needed to complete the operation.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="profileId">ID fo the web profile to partially update.</param>
    /// <param name="patchRequest">PatchRequest</param>
    public static void PartialUpdate(
        APIContext apiContext,
        string profileId,
        PatchRequest patchRequest)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(profileId, nameof(profileId));
        ArgumentValidator.Validate(patchRequest, nameof(patchRequest));
        var resource = SdkUtil.FormatUriPath("v1/payment-experience/web-profiles/{0}", [
            profileId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Patch, resource, patchRequest);
    }

    /// <summary>Shows details for a web experience profile, by ID.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="profileId">The ID of the profile for which to show details.</param>
    /// <returns>WebProfile</returns>
    public static WebProfile Get(APIContext apiContext, string profileId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(profileId, nameof(profileId));
        var resource = SdkUtil.FormatUriPath("v1/payment-experience/web-profiles/{0}", [
            profileId,
        ]);
        return ConfigureAndExecute<WebProfile>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Lists all web experience profiles for a merchant or subject.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>WebProfileList</returns>
    public static WebProfileList GetList(APIContext apiContext)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        const string resource = "v1/payment-experience/web-profiles";
        return ConfigureAndExecute<WebProfileList>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>Deletes a web experience profile, by ID.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    public void Delete(APIContext apiContext)
    {
        Delete(apiContext, Id);
    }

    /// <summary>Deletes a web experience profile, by ID.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="profileId">The ID of the profile to delete.</param>
    public static void Delete(APIContext apiContext, string profileId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(profileId, nameof(profileId));
        apiContext.MaskRequestId = true;
        var resource = SdkUtil.FormatUriPath("v1/payment-experience/web-profiles/{0}", [
            profileId,
        ]);
        ConfigureAndExecute<WebProfile>(apiContext, HttpMethod.Delete, resource);
    }
}