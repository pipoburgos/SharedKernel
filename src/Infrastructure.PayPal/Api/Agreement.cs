using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A REST API billing agreement resource.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class Agreement : PayPalRelationalObject
{
    /// <summary>Identifier of the agreement.</summary>
    public string Id { get; set; }

    /// <summary>State of the agreement.</summary>
    public string State { get; set; }

    /// <summary>Name of the agreement.</summary>
    public string Name { get; set; }

    /// <summary>Description of the agreement.</summary>
    public string Description { get; set; }

    /// <summary>
    /// Start date of the agreement. Date format yyyy-MM-dd z, as defined in [ISO8601](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string StartDate { get; set; }

    /// <summary>Details of the agreement.</summary>
    public AgreementDetails AgreementDetails { get; set; }

    /// <summary>
    /// Details of the buyer who is enrolling in this agreement. This information is gathered from execution of the approval URL.
    /// </summary>
    public Payer Payer { get; set; }

    /// <summary>
    /// Shipping address object of the agreement, which should be provided if it is different from the default address.
    /// </summary>
    public Address ShippingAddress { get; set; }

    /// <summary>
    /// Default merchant preferences from the billing plan are used, unless override preferences are provided here.
    /// </summary>
    public MerchantPreferences OverrideMerchantPreferences { get; set; }

    /// <summary>
    /// Array of override_charge_model for this agreement if needed to change the default models from the billing plan.
    /// </summary>
    public List<OverrideChargeModel> OverrideChargeModels { get; set; }

    /// <summary>Plan details for this agreement.</summary>
    public Plan Plan { get; set; }

    /// <summary>
    /// Date and time that this resource was created. Date format yyyy-MM-dd z, as defined in [ISO8601](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string CreateTime { get; set; }

    /// <summary>
    /// Date and time that this resource was updated. Date format yyyy-MM-dd z, as defined in [ISO8601](http://tools.ietf.org/html/rfc3339#section-5.6).
    /// </summary>
    public string UpdateTime { get; set; }

    /// <summary>
    /// Get or sets the token found in the approval_url link returned from a call to create this resource.
    /// </summary>
    //[JsonIgnore]
    public string Token { private get; set; }

    /// <summary>
    /// Create a new billing agreement by passing the details for the agreement, including the name, description, start date, payer, and billing plan in the request JSON.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>Agreement</returns>
    public Agreement Create(APIContext apiContext) => Create(apiContext, this);

    /// <summary>
    /// Create a new billing agreement by passing the details for the agreement, including the name, description, start date, payer, and billing plan in the request JSON.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreement">The Agreement object to be used when creating the PayPal resource.</param>
    /// <returns>Agreement</returns>
    public static Agreement Create(APIContext apiContext, Agreement agreement)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        const string resource = "v1/payments/billing-agreements";
        var agreement1 = ConfigureAndExecute(apiContext, HttpMethod.Post, resource, agreement);
        agreement1.Token = agreement1.GetTokenFromApprovalUrl();
        return agreement1;
    }

    /// <summary>
    /// Execute a billing agreement after buyer approval by passing the payment token to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>Agreement</returns>
    public Agreement Execute(APIContext apiContext) => Execute(apiContext, Token);

    /// <summary>
    /// Execute a billing agreement after buyer approval by passing the payment token to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="token">Payment token received after buyer approval of the billing agreement.</param>
    /// <returns>Agreement</returns>
    public static Agreement Execute(APIContext apiContext, string token)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-agreements/{0}/agreement-execute", [
            token,
        ]);
        return ConfigureAndExecute<Agreement>(apiContext, HttpMethod.Post, resource);
    }

    /// <summary>
    /// Retrieve details for a particular billing agreement by passing the ID of the agreement to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementId">Identifier of the agreement resource to retrieve.</param>
    /// <returns>Agreement</returns>
    public static Agreement Get(APIContext apiContext, string agreementId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(agreementId, nameof(agreementId));
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-agreements/{0}", [
            agreementId,
        ]);
        return ConfigureAndExecute<Agreement>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Update details of a billing agreement, such as the description, shipping address, and start date, by passing the ID of the agreement to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="patchRequest">PatchRequest</param>
    public void Update(APIContext apiContext, PatchRequest patchRequest)
    {
        Update(apiContext, Id, patchRequest);
    }

    /// <summary>
    /// Update details of a billing agreement, such as the description, shipping address, and start date, by passing the ID of the agreement to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementId">ID of the billing agreement that will be updated.</param>
    /// <param name="patchRequest">PatchRequest</param>
    public static void Update(APIContext apiContext, string agreementId, PatchRequest patchRequest)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(agreementId, nameof(agreementId));
        ArgumentValidator.Validate(patchRequest, nameof(patchRequest));
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-agreements/{0}", [
            agreementId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Patch, resource, patchRequest);
    }

    /// <summary>
    /// Suspend a particular billing agreement by passing the ID of the agreement to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementStateDescriptor">AgreementStateDescriptor</param>
    public void Suspend(APIContext apiContext, AgreementStateDescriptor agreementStateDescriptor)
    {
        Suspend(apiContext, Id, agreementStateDescriptor);
    }

    /// <summary>
    /// Suspend a particular billing agreement by passing the ID of the agreement to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementId">ID of the billing agreement that will be suspended.</param>
    /// <param name="agreementStateDescriptor">AgreementStateDescriptor</param>
    public static void Suspend(
        APIContext apiContext,
        string agreementId,
        AgreementStateDescriptor agreementStateDescriptor)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(agreementId, nameof(agreementId));
        ArgumentValidator.Validate(agreementStateDescriptor, nameof(agreementStateDescriptor));
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-agreements/{0}/suspend", [
            agreementId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Post, resource, agreementStateDescriptor);
    }

    /// <summary>
    /// Reactivate a suspended billing agreement by passing the ID of the agreement to the appropriate URI. In addition, pass an AgreementStateDescriptor object in the request JSON that includes a note about the reason for changing the state of the agreement and the amount and currency for the agreement.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementStateDescriptor">AgreementStateDescriptor</param>
    public void ReActivate(APIContext apiContext, AgreementStateDescriptor agreementStateDescriptor)
    {
        ReActivate(apiContext, Id, agreementStateDescriptor);
    }

    /// <summary>
    /// Reactivate a suspended billing agreement by passing the ID of the agreement to the appropriate URI. In addition, pass an AgreementStateDescriptor object in the request JSON that includes a note about the reason for changing the state of the agreement and the amount and currency for the agreement.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementId">ID of the billing agreement that will be reactivated.</param>
    /// <param name="agreementStateDescriptor">AgreementStateDescriptor</param>
    public static void ReActivate(
        APIContext apiContext,
        string agreementId,
        AgreementStateDescriptor agreementStateDescriptor)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(agreementId, nameof(agreementId));
        ArgumentValidator.Validate(agreementStateDescriptor, nameof(agreementStateDescriptor));
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-agreements/{0}/re-activate", [
            agreementId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Post, resource, agreementStateDescriptor);
    }

    /// <summary>
    /// Cancel a billing agreement by passing the ID of the agreement to the request URI. In addition, pass an agreement_state_descriptor object in the request JSON that includes a note about the reason for changing the state of the agreement and the amount and currency for the agreement.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementStateDescriptor">AgreementStateDescriptor</param>
    public void Cancel(APIContext apiContext, AgreementStateDescriptor agreementStateDescriptor)
    {
        Cancel(apiContext, Id, agreementStateDescriptor);
    }

    /// <summary>
    /// Cancel a billing agreement by passing the ID of the agreement to the request URI. In addition, pass an AgreementStateDescriptor object in the request JSON that includes a note about the reason for changing the state of the agreement and the amount and currency for the agreement.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementId">ID of the billing agreement that will be canceled.</param>
    /// <param name="agreementStateDescriptor">AgreementStateDescriptor</param>
    public static void Cancel(
        APIContext apiContext,
        string agreementId,
        AgreementStateDescriptor agreementStateDescriptor)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(agreementId, nameof(agreementId));
        ArgumentValidator.Validate(agreementStateDescriptor, nameof(agreementStateDescriptor));
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-agreements/{0}/cancel", [
            agreementId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Post, resource, agreementStateDescriptor);
    }

    /// <summary>
    /// Bill an outstanding amount for an agreement by passing the ID of the agreement to the request URI. In addition, pass an AgreementStateDescriptor object in the request JSON that includes a note about the reason for changing the state of the agreement and the amount and currency for the agreement.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementStateDescriptor">AgreementStateDescriptor</param>
    public void BillBalance(
        APIContext apiContext,
        AgreementStateDescriptor agreementStateDescriptor)
    {
        BillBalance(apiContext, Id, agreementStateDescriptor);
    }

    /// <summary>
    /// Bill an outstanding amount for an agreement by passing the ID of the agreement to the request URI. In addition, pass an AgreementStateDescriptor object in the request JSON that includes a note about the reason for changing the state of the agreement and the amount and currency for the agreement.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementId">ID of the billing agreement to perform the operation against.</param>
    /// <param name="agreementStateDescriptor">AgreementStateDescriptor</param>
    public static void BillBalance(
        APIContext apiContext,
        string agreementId,
        AgreementStateDescriptor agreementStateDescriptor)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(agreementId, nameof(agreementId));
        ArgumentValidator.Validate(agreementStateDescriptor, nameof(agreementStateDescriptor));
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-agreements/{0}/bill-balance", [
            agreementId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Post, resource, agreementStateDescriptor);
    }

    /// <summary>
    /// Set the balance for an agreement by passing the ID of the agreement to the request URI. In addition, pass a Currency object in the request JSON that specifies the currency type and value of the balance.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="currency">Currency</param>
    public void SetBalance(APIContext apiContext, PayPalCurrency currency)
    {
        SetBalance(apiContext, Id, currency);
    }

    /// <summary>
    /// Set the balance for an agreement by passing the ID of the agreement to the request URI. In addition, pass a Currency object in the request JSON that specifies the currency type and value of the balance.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementId">ID of the billing agreement to perform the operation against.</param>
    /// <param name="currency">Currency</param>
    public static void SetBalance(APIContext apiContext, string agreementId, PayPalCurrency currency)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(agreementId, nameof(agreementId));
        ArgumentValidator.Validate(currency, nameof(currency));
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-agreements/{0}/set-balance", [
            agreementId,
        ]);
        ConfigureAndExecute(apiContext, HttpMethod.Post, resource, currency);
    }

    /// <summary>
    /// List transactions for a billing agreement by passing the ID of the agreement, as well as the start and end dates of the range of transactions to list, to the request URI.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="agreementId">Identifier of the agreement resource for which to list transactions.</param>
    /// <param name="startDate">The start date of the range of transactions to list. Date format must be yyyy-MM-dd.</param>
    /// <param name="endDate">The end date of the range of transactions to list. Date format must be yyyy-MM-dd.</param>
    /// <returns>AgreementTransactions</returns>
    public static AgreementTransactions ListTransactions(
        APIContext apiContext,
        string agreementId,
        string startDate,
        string endDate)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(agreementId, nameof(agreementId));
        ArgumentValidator.Validate(startDate, nameof(startDate));
        ArgumentValidator.Validate(endDate, nameof(endDate));
        var queryParameters = new QueryParameters
        {
            ["start_date"] = startDate,
            ["end_date"] = endDate,
        };
        var resource = SdkUtil.FormatUriPath("v1/payments/billing-agreements/{0}/transactions", [
            agreementId,
        ]) + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<AgreementTransactions>(apiContext, HttpMethod.Get, resource);
    }
}