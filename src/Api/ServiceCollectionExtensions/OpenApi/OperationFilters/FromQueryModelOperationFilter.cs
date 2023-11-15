using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.OpenApi.Models;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Queries;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi.OperationFilters;

/// <summary>
/// 
/// </summary>
public class FromQueryModelOperationFilter : IOperationFilter
{
    /// <summary>
    /// Fix how shows Swashbuckle complex types in FromQuery params
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var description = context.ApiDescription;
        // We only want to do this for GET requests, if this is not a GET request, leave this operation as is, do not modify
        if (description.HttpMethod?.ToLower() != HttpMethod.Get.ToString().ToLower())
            return;

        var actionParameters = description.ActionDescriptor.Parameters;
        var apiParameters = description.ParameterDescriptions
            .Where(p => p.Source.IsFromRequest)
            .ToList();

        // If no complex query parameters detected, leave this operation as is, do not modify
        if (actionParameters.Count == apiParameters.Count)
            return;

        var parameters = CreateParameters(actionParameters, operation.Parameters, context);

        if (parameters == default || !parameters.Any())
            return;

        operation.Parameters = parameters;
    }

    private IList<OpenApiParameter?>? CreateParameters(IList<ParameterDescriptor> actionParameters,
        IList<OpenApiParameter> operationParameters, OperationFilterContext context)
    {
        var newParameters = actionParameters
            .Select(p => CreateParameter(p, operationParameters, context))
            .Where(p => p != default)
            .ToList();

        return newParameters.Any() ? newParameters : default;
    }

    private OpenApiParameter? CreateParameter(ParameterDescriptor actionParameter,
        IList<OpenApiParameter> operationParameters, OperationFilterContext context)
    {
        if (actionParameter.ParameterType == typeof(CancellationToken))
            return default;

        if (actionParameter.ParameterType == typeof(IQueryBus))
            return default;

        if (actionParameter.ParameterType == typeof(ICommandBus))
            return default;

        if (actionParameter.ParameterType.IsClass && !actionParameter.ParameterType.GetProperties().Any())
            return default;

        var operationParamNames = operationParameters.Select(p => p.Name);
        if (operationParamNames.Contains(actionParameter.Name))
        {
            // If param is defined as the action method argument, just pass it through
            return operationParameters.First(p => p.Name == actionParameter.Name);
        }

#if !NET5_0
        if (actionParameter.BindingInfo == default)
        {
            return default;
        }
#endif

        var generatedSchema = context.SchemaGenerator
            .GenerateSchema(actionParameter.ParameterType, context.SchemaRepository);

        var newParameter = new OpenApiParameter
        {
            Name = actionParameter.Name,
            In = ParameterLocation.Query,
            Schema = generatedSchema
        };

        return newParameter;
    }
}