using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// [DEPRECATED] Represents a credit card to fund a payment. Use Payment Card instead.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class CreditCard : PayPalRelationalObject
{
    /// <summary>ID of the credit card being saved for later use.</summary>
    public string Id { get; set; }

    /// <summary>
    /// Credit card number. Numeric characters only with no spaces or punctuation. The string must conform with modulo and length required by each credit card type. *Redacted in responses.*
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Credit card type. Valid types are: `visa`, `mastercard`, `discover`, `amex`
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Expiration month with no leading zero. Acceptable values are 1 through 12.
    /// </summary>
    public int ExpireMonth { get; set; }

    /// <summary>4-digit expiration year.</summary>
    public int ExpireYear { get; set; }

    /// <summary>3-4 digit card validation code.</summary>
    public string Cvv2 { get; set; }

    /// <summary>Cardholder's first name.</summary>
    public string FirstName { get; set; }

    /// <summary>Cardholder's last name.</summary>
    public string LastName { get; set; }

    /// <summary>Billing Address associated with this card.</summary>
    public Address BillingAddress { get; set; }

    /// <summary>
    /// A unique identifier of the payer generated and provided by the facilitator. This is required when creating or using a tokenized funding instrument.
    /// </summary>
    public string PayerId { get; set; }

    /// <summary>
    /// A unique identifier of the customer to whom this bank account belongs. Generated and provided by the facilitator. **This is now used in favor of `payer_id` when creating or using a stored funding instrument in the vault.**
    /// </summary>
    public string ExternalCustomerId { get; set; }

    /// <summary>
    /// A user provided, optional convenvience field that functions as a unique identifier for the merchant on behalf of whom this credit card is being stored for. Note that this has no relation to PayPal merchant id
    /// </summary>
    public string MerchantId { get; set; }

    /// <summary>
    /// A unique identifier of the bank account resource. Generated and provided by the facilitator so it can be used to restrict the usage of the bank account to the specific merchant.
    /// </summary>
    public string ExternalCardId { get; set; }

    /// <summary>State of the credit card funding instrument.</summary>
    public string State { get; set; }

    /// <summary>
    /// Resource creation time  as ISO8601 date-time format (ex: 1994-11-05T13:15:30Z) that indicates creation time.
    /// </summary>
    public string CreateTime { get; set; }

    /// <summary>
    /// Resource creation time  as ISO8601 date-time format (ex: 1994-11-05T13:15:30Z) that indicates the updation time.
    /// </summary>
    public string UpdateTime { get; set; }

    /// <summary>Funding instrument expiration date.</summary>
    public string ValidUntil { get; set; }

    /// <summary>Creates a new Credit Card Resource (aka Tokenize).</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>CreditCard</returns>
    public CreditCard Create(APIContext apiContext) => Create(apiContext, this);

    /// <summary>Creates a new Credit Card Resource (aka Tokenize).</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="creditCard">CreditCard object to be used to create the PayPal resource.</param>
    /// <returns>CreditCard</returns>
    public static CreditCard Create(APIContext apiContext, CreditCard creditCard)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        const string resource = "v1/vault/credit-cards";
        return ConfigureAndExecute(apiContext, HttpMethod.Post, resource, creditCard);
    }

    /// <summary>
    /// Obtain the Credit Card resource for the given identifier.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="creditCardId">Identifier of the credit card resource to obtain the data for.</param>
    /// <returns>CreditCard</returns>
    public static CreditCard Get(APIContext apiContext, string creditCardId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(creditCardId, nameof(creditCardId));
        var resource = SdkUtil.FormatUriPath("v1/vault/credit-cards/{0}", [
            creditCardId,
        ]);
        return ConfigureAndExecute<CreditCard>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Delete the Credit Card resource for the given identifier.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    public void Delete(APIContext apiContext) => Delete(apiContext, Id);

    /// <summary>
    /// Delete the Credit Card resource for the given identifier.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="creditCardId">Identifier of the credit card resource to obtain the data for.</param>
    public static void Delete(APIContext apiContext, string creditCardId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(creditCardId, nameof(creditCardId));
        apiContext.MaskRequestId = true;
        var resource = SdkUtil.FormatUriPath("v1/vault/credit-cards/{0}", [
            creditCardId,
        ]);
        ConfigureAndExecute<CreditCard>(apiContext, HttpMethod.Delete, resource);
    }

    /// <summary>
    /// Update information in a previously saved card. Only the modified fields need to be passed in the request.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="patchRequest">PatchRequest</param>
    /// <returns>CreditCard</returns>
    public CreditCard Update(APIContext apiContext, PatchRequest patchRequest)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(Id, "Id");
        ArgumentValidator.Validate(patchRequest, nameof(patchRequest));
        var resource = SdkUtil.FormatUriPath("v1/vault/credit-cards/{0}", [
            Id,
        ]);
        return ConfigureAndExecute<CreditCard>(apiContext, HttpMethod.Patch, resource, patchRequest);
    }

    /// <summary>
    /// Update information in a previously saved card. Only the modified fields need to be passed in the request.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="creditCardId">ID fo the credit card to update.</param>
    /// <param name="patchRequest">PatchRequest</param>
    /// <returns>CreditCard</returns>
    public static CreditCard Update(
        APIContext apiContext,
        string creditCardId,
        PatchRequest patchRequest)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(creditCardId, nameof(creditCardId));
        ArgumentValidator.Validate(patchRequest, nameof(patchRequest));
        var resource = SdkUtil.FormatUriPath("v1/vault/credit-cards/{0}", [
            creditCardId,
        ]);
        return ConfigureAndExecute<CreditCard>(apiContext, HttpMethod.Patch, resource, patchRequest);
    }

    /// <summary>Retrieves a list of Credit Card resources.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="pageSize">Number of items to be returned in the current page size, by a GET operation.</param>
    /// <param name="page">The page number to be retrieved, for the list of items, by the current GET request.</param>
    /// <param name="startTime">Resource creation time  as ISO8601 date-time format (ex: 1994-11-05T13:15:30Z) that indicates the start of a range of results.</param>
    /// <param name="endTime">Resource creation time as ISO8601 date-time format (ex: 1994-11-05T13:15:30Z) that indicates the end of a range of results.</param>
    /// <param name="sortOrder">Sort based on order of results. Options include 'asc' for ascending order or 'desc' for descending order.</param>
    /// <param name="sortBy">Sort based on 'create_time' or 'update_time'.</param>
    /// <param name="merchantId">Merchant identifier to filter the search results in list operations.</param>
    /// <param name="externalCardId">Externally provided card identifier to filter the search results in list operations.</param>
    /// <param name="externalCustomerId">Externally provided customer identifier to filter the search results in list operations.</param>
    /// <param name="totalRequired">Identifies if total count is required or not. Defaults to true.</param>
    /// <returns>CreditCardList</returns>
    public static CreditCardList List(
        APIContext apiContext,
        int pageSize = 10,
        int page = 1,
        string startTime = "",
        string endTime = "",
        string sortOrder = "asc",
        string sortBy = "create_time",
        string merchantId = "",
        string externalCardId = "",
        string externalCustomerId = "",
        bool totalRequired = true)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        var queryParameters = new QueryParameters
        {
            ["page_size"] = pageSize.ToString(),
            [nameof(page)] = page.ToString(),
            ["start_time"] = startTime,
            ["end_time"] = endTime,
            ["sort_order"] = sortOrder,
            ["sort_by"] = sortBy,
            ["merchant_id"] = merchantId,
            ["external_card_id"] = externalCardId,
            ["external_customer_id"] = externalCustomerId,
            ["total_required"] = totalRequired.ToString(),
        };
        var resource = "v1/vault/credit-cards" + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<CreditCardList>(apiContext, HttpMethod.Get, resource);
    }
}