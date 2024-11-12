using SharedKernel.Infrastructure.PayPal.Util;

namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Invoicing template.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class InvoiceTemplate : PayPalResource
{
    /// <summary>The ID of the template.</summary>
    public string TemplateId { get; set; }

    /// <summary>The template name.</summary>
    public string Name { get; set; }

    /// <summary>
    /// Indicates whether this template is the default merchant template. A merchant can have one default template.
    /// </summary>
    public bool? Default { get; set; }

    /// <summary>
    /// Customized invoice data, which is saved as the template.
    /// </summary>
    public InvoiceTemplateData TemplateData { get; set; }

    /// <summary>Settings for each template.</summary>
    public List<InvoiceTemplateSettings> Settings { get; set; }

    /// <summary>
    /// The unit of measure for the template. Value is quantity, hours, or amount.
    /// </summary>
    public string UnitOfMeasure { get; set; }

    /// <summary>
    /// Indicates whether this template is a merchant-created custom template. Non-custom templates are system generated.
    /// </summary>
    public bool? Custom { get; set; }

    /// <summary>Shows details for a template, by ID.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="templateId">The ID of the template for which to show details.</param>
    /// <returns>Template</returns>
    public static InvoiceTemplate Get(IPayPalClient apiContext, string templateId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(templateId, nameof(templateId));
        var resource = SdkUtil.FormatUriPath("v1/invoicing/templates/{0}", [
            templateId,
        ]);
        return ConfigureAndExecute<InvoiceTemplate>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>
    /// Lists all merchant-created templates. The list shows the emails, addresses, and phone numbers from the merchant profile.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="fields">The fields to return in the response. Value is `all` or `none`. Specify `none` to return only the template name, ID, and default attributes.</param>
    /// <returns>Templates</returns>
    public static InvoiceTemplates GetAll(IPayPalClient apiContext, string fields = "all")
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        var queryParameters = new QueryParameters
        {
            [nameof(fields)] = fields,
        };
        var resource = "v1/invoicing/templates" + queryParameters.ToUrlFormattedString();
        return ConfigureAndExecute<InvoiceTemplates>(apiContext, HttpMethod.Get, resource);
    }

    /// <summary>Deletes a template, by ID.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    public void Delete(IPayPalClient apiContext)
    {
        Delete(apiContext, TemplateId);
    }

    /// <summary>Deletes a template, by ID.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <param name="templateId">The ID of the template to delete.</param>
    public static void Delete(IPayPalClient apiContext, string templateId)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(templateId, nameof(templateId));
        apiContext.MaskRequestId = true;
        var resource = SdkUtil.FormatUriPath("v1/invoicing/templates/{0}", [
            templateId,
        ]);
        ConfigureAndExecute<InvoiceTemplate>(apiContext, HttpMethod.Delete, resource);
    }

    /// <summary>Creates a template.</summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <returns>Template</returns>
    public InvoiceTemplate Create(IPayPalClient apiContext)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(this, "template");
        const string resource = "v1/invoicing/templates";
        return ConfigureAndExecute(apiContext, HttpMethod.Post, resource, this);
    }

    /// <summary>
    /// Updates a template, by ID. In the JSON request body, pass a complete `template` object. The update method does not support partial updates.
    /// </summary>
    /// <param name="apiContext">IPayPalClient used for the API call.</param>
    /// <returns>Template</returns>
    public InvoiceTemplate Update(IPayPalClient apiContext)
    {
        ArgumentValidator.ValidateAndSetupApiContext(apiContext);
        ArgumentValidator.Validate(this, "template");
        var resource = SdkUtil.FormatUriPath("v1/invoicing/templates/{0}", [
            TemplateId,
        ]);
        return ConfigureAndExecute(apiContext, HttpMethod.Put, resource, this);
    }
}