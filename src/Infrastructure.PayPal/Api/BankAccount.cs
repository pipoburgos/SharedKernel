using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// A resource representing a bank account that can be used to fund a payment.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class BankAccount : PayPalRelationalObject
{
    /// <summary>ID of the bank account being saved for later use.</summary>
    public string Id { get; set; }

    /// <summary>
    /// Account number in either IBAN (max length 34) or BBAN (max length 17) format.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// Type of the bank account number (International or Basic Bank Account Number). For more information refer to http://en.wikipedia.org/wiki/International_Bank_Account_Number.
    /// </summary>
    public string AccountNumberType { get; set; }

    /// <summary>
    /// Routing transit number (aka Bank Code) of the bank (typically for domestic use only - for international use, IBAN includes bank code). For more information refer to http://en.wikipedia.org/wiki/Bank_code.
    /// </summary>
    public string RoutingNumber { get; set; }

    /// <summary>Type of the bank account.</summary>
    public string AccountType { get; set; }

    /// <summary>A customer designated name.</summary>
    public string AccountName { get; set; }

    /// <summary>
    /// Type of the check when this information was obtained through a check by the facilitator or merchant.
    /// </summary>
    public string CheckType { get; set; }

    /// <summary>
    /// How the check was obtained from the customer, if check was the source of the information provided.
    /// </summary>
    public string AuthType { get; set; }

    /// <summary>
    /// Time at which the authorization (or check) was captured. Use this field if the user authorization needs to be captured due to any privacy requirements.
    /// </summary>
    public string AuthCaptureTimestamp { get; set; }

    /// <summary>Name of the bank.</summary>
    public string BankName { get; set; }

    /// <summary>2 letter country code of the Bank.</summary>
    public string CountryCode { get; set; }

    /// <summary>Account holder's first name.</summary>
    public string FirstName { get; set; }

    /// <summary>Account holder's last name.</summary>
    public string LastName { get; set; }

    /// <summary>Birth date of the bank account holder.</summary>
    public string BirthDate { get; set; }

    /// <summary>Billing address.</summary>
    public Address BillingAddress { get; set; }

    /// <summary>State of this funding instrument.</summary>
    public string State { get; set; }

    /// <summary>Confirmation status of a bank account.</summary>
    public string ConfirmationStatus { get; set; }

    /// <summary>Deprecated - Use external_customer_id instead.</summary>
    public string PayerId { get; set; }

    /// <summary>
    /// A unique identifier of the customer to whom this bank account belongs to. Generated and provided by the facilitator. This is required when creating or using a stored funding instrument in vault.
    /// </summary>
    public string ExternalCustomerId { get; set; }

    /// <summary>
    /// A unique identifier of the merchant for which this bank account has been stored for. Generated and provided by the facilitator so it can be used to restrict the usage of the bank account to the specific merchant.
    /// </summary>
    public string MerchantId { get; set; }

    /// <summary>
    /// A client supplied unique identifier of the bank account resource, to faciliate easy look up of the resource, via GET queries
    /// </summary>
    public string ExternalAccountId { get; set; }

    /// <summary>Time the resource was created.</summary>
    public string CreateTime { get; set; }

    /// <summary>Time the resource was last updated.</summary>
    public string UpdateTime { get; set; }

    /// <summary>
    /// Date/Time until this resource can be used to fund a payment.
    /// </summary>
    public string ValidUntil { get; set; }

    /// <summary>Creates a new Bank Account Resource.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <returns>BankAccount</returns>
    public BankAccount Create(APIContext apiContext) => Create(apiContext, this);

    /// <summary>Creates a new Bank Account Resource.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="bankAccount">The BankAccount object specifying the details of the PayPal resource to create.</param>
    /// <returns>BankAccount</returns>
    public static BankAccount Create(APIContext apiContext, BankAccount bankAccount)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        const string resource = "v1/vault/bank-accounts";
        return ConfigureAndExecute(apiContext, HttpMethod.Post, resource, bankAccount);
    }

    /// <summary>
    /// Obtain the Bank Account resource for the given identifier.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="bankAccountId">Identifier of the bank account resource to obtain the data for.</param>
    /// <returns>BankAccount</returns>
    public static BankAccount Get(APIContext apiContext, string bankAccountId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(bankAccountId, nameof(bankAccountId));
        var resource = SdkUtil.FormatUriPath("v1/vault/bank-accounts/{0}", [
            bankAccountId,
        ]);
        return ConfigureAndExecute<BankAccount>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Delete the bank account resource for the given identifier.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    public void Delete(APIContext apiContext) => Delete(apiContext, Id);

    /// <summary>
    /// Delete the bank account resource for the given identifier.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="bankAccountId">Identifier of the bank account resource to obtain the data for.</param>
    public static void Delete(APIContext apiContext, string bankAccountId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(bankAccountId, nameof(bankAccountId));
        apiContext.MaskRequestId = true;
        var resource = SdkUtil.FormatUriPath("v1/vault/bank-accounts/{0}", [
            bankAccountId,
        ]);
        ConfigureAndExecute<BankAccount>(apiContext, HttpMethod.Delete, resource);
    }

    /// <summary>
    /// Update information in a previously saved bank account. Only the modified fields need to be passed in the request.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="patchRequest">PatchRequest</param>
    /// <returns>BankAccount</returns>
    public BankAccount Update(APIContext apiContext, PatchRequest patchRequest)
    {
        return Update(apiContext, Id, patchRequest);
    }

    /// <summary>
    /// Update information in a previously saved bank account. Only the modified fields need to be passed in the request.
    /// </summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="bankAccountId">ID of the bank account to update.</param>
    /// <param name="patchRequest">PatchRequest</param>
    /// <returns>BankAccount</returns>
    public static BankAccount Update(
        APIContext apiContext,
        string bankAccountId,
        PatchRequest patchRequest)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(bankAccountId, nameof(bankAccountId));
        ArgumentValidator.Validate(patchRequest, nameof(patchRequest));
        var resource = SdkUtil.FormatUriPath("v1/vault/bank-accounts/{0}", [
            bankAccountId,
        ]);
        return ConfigureAndExecute<BankAccount>(apiContext, HttpMethod.Patch, resource, patchRequest);
    }

    /// <summary>Retrieves a list of bank account resources.</summary>
    /// <param name="apiContext">APIContext used for the API call.</param>
    /// <param name="pageSize">Number of items to be returned in the current page size, by a GET operation. Defaults to a size of 10.</param>
    /// <param name="page">The page number to be retrieved, for the list of items, by the current GET request. Defaults to a size of 1.</param>
    /// <param name="startTime">Resource creation time  as ISO8601 date-time format (ex: 1994-11-05T13:15:30Z) that indicates the start of a range of results.</param>
    /// <param name="endTime">Resource creation time as ISO8601 date-time format (ex: 1994-11-05T13:15:30Z) that indicates the end of a range of results.</param>
    /// <param name="sortOrder">Sort based on order of results. Options include 'asc' for ascending order or 'desc' for descending order. Defaults to 'asc'.</param>
    /// <param name="sortBy">Sort based on 'create_time' or 'update_time'. Defaults to 'create_time'.</param>
    /// <param name="merchantId">Identifier the merchants who owns this resource</param>
    /// <param name="externalCustomerId">Identifier of the external customer resource to obtain the data for.</param>
    /// <param name="externalAccountId">Identifier of the external bank account resource id to obtain the data for.</param>
    /// <returns>BankAccountList</returns>
    public static BankAccountList List(
        APIContext apiContext,
        int pageSize = 10,
        int page = 1,
        string startTime = "",
        string endTime = "",
        string sortOrder = "asc",
        string sortBy = "create_time",
        string merchantId = "",
        string externalCustomerId = "",
        string externalAccountId = "")
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
            ["external_customer_id"] = externalCustomerId,
            ["external_account_id"] = externalAccountId,
        };
        var resource = "v1/vault/bank-accounts" + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<BankAccountList>(apiContext, HttpMethod.Get, resource);
    }
}