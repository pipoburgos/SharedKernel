using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A REST API billing plan resource.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Plan : PayPalRelationalObject
{
    /// <summary>Identifier of the billing plan. 128 characters max.</summary>
    public string Id { get; set; }

    /// <summary>Name of the billing plan. 128 characters max.</summary>
    public string Name { get; set; }

    /// <summary>Description of the billing plan. 128 characters max.</summary>
    public string Description { get; set; }

    /// <summary>
    /// Type of the billing plan. Allowed values: `FIXED`, `INFINITE`.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Status of the billing plan. Allowed values: `CREATED`, `ACTIVE`, `INACTIVE`, and `DELETED`.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Time when the billing plan was created. Format YYYY-MM-DDTimeTimezone, as defined in [ISO8601](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string CreateTime { get; set; }

    /// <summary>
    /// Time when this billing plan was updated. Format YYYY-MM-DDTimeTimezone, as defined in [ISO8601](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string UpdateTime { get; set; }

    /// <summary>Array of payment definitions for this billing plan.</summary>
    public List<PaymentDefinition> PaymentDefinitions { get; set; }

    /// <summary>Array of terms for this billing plan.</summary>
    public List<Terms> Terms { get; set; }

    /// <summary>
    /// Specific preferences such as: set up fee, max fail attempts, autobill amount, and others that are configured for this billing plan.
    /// </summary>
    public MerchantPreferences MerchantPreferences { get; set; }

    /// <summary>
    /// Retrieve the details for a particular billing plan by passing the billing plan ID to the request URI.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="planId">ID of the billing plan to return details about.</param>
    /// <returns>Plan</returns>
    public static Plan Get(IPayPalClient apiContext, string planId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(planId, nameof(planId));
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-plans/{0}", [
            planId,
        ]);
        return ConfigureAndExecute<Plan>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Create a new billing plan by passing the details for the plan, including the plan name, description, and type, to the request URI.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <returns>Plan</returns>
    public Plan Create(IPayPalClient apiContext)
    {
        return Create(apiContext, this);
    }

    /// <summary>
    /// Create a new billing plan by passing the details for the plan, including the plan name, description, and type, to the request URI.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="plan">The Plan object to be used to create the billing plan resource.</param>
    /// <returns>Plan</returns>
    public static Plan Create(IPayPalClient apiContext, Plan plan)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        const string resource = "v1/payments/billing-plans";
        return ConfigureAndExecute(apiContext, HttpMethod.Post, resource, plan);
    }

    /// <summary>
    /// Replace specific fields within a billing plan by passing the ID of the billing plan to the request URI. In addition, pass a patch object in the request JSON that specifies the operation to perform, field to update, and new value for each update.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="patchRequest">PatchRequest</param>
    public void Update(IPayPalClient apiContext, PatchRequest patchRequest)
    {
        Update(apiContext, Id, patchRequest);
    }

    /// <summary>
    /// Replace specific fields within a billing plan by passing the ID of the billing plan to the request URI. In addition, pass a patch object in the request JSON that specifies the operation to perform, field to update, and new value for each update.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="planId">ID of the billing plan to be updated.</param>
    /// <param name="patchRequest">PatchRequest</param>
    public static void Update(IPayPalClient apiContext, string planId, PatchRequest patchRequest)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(planId, nameof(planId));
        ArgumentValidator.Validate(patchRequest, nameof(patchRequest));
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-plans/{0}", [
            planId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Patch, resource, patchRequest);
    }

    /// <summary>
    /// List billing plans according to optional query string parameters specified.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="page">A non-zero integer representing the 'page' of the results.</param>
    /// <param name="status">Specifies the status (CREATED, ACTIVE, or INACTIVE) of the billing plans to return.</param>
    /// <param name="pageSize">A non-negative, non-zero integer indicating the maximum number of results to return at one time.</param>
    /// <param name="totalRequired">A boolean indicating total number of items (total_items) and pages (total_pages) are expected to be returned in the response</param>
    /// <returns>PlanList</returns>
    public static PlanList List(
        IPayPalClient apiContext,
        string page = "0",
        string status = "CREATED",
        string pageSize = "10",
        string totalRequired = "yes")
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        var queryParameters = new QueryParameters
        {
            [nameof(page)] = page,
            [nameof(status)] = status,
            ["page_size"] = pageSize,
            ["total_required"] = totalRequired,
        };
        var resource = "v1/payments/billing-plans" + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<PlanList>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>Deletes the specified billing plan.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    public void Delete(IPayPalClient apiContext)
    {
        Delete(apiContext, Id);
    }

    /// <summary>Deletes the specified billing plan.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="planId">ID of the billing plan to delete.</param>
    public static void Delete(IPayPalClient apiContext, string planId)
    {
        var patchRequest1 = new PatchRequest
        {
            new Patch
            {
                Op = "replace",
                Path = "/",
                Value = new Plan { State = "DELETED" },
            },
        };
        var patchRequest2 = patchRequest1;
        Update(apiContext, planId, patchRequest2);
    }
}