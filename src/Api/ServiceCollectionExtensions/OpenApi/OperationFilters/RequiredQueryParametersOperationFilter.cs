using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi.OperationFilters;

public class RequiredQueryParametersOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            return;

        var methodParams = context.MethodInfo.GetParameters();

        foreach (var opParam in operation.Parameters)
        {
            var param = opParam;
            var clrParam = methodParams.FirstOrDefault(p =>
                string.Equals(p.Name, param.Name, StringComparison.OrdinalIgnoreCase));

            if (clrParam == null)
                continue;

            if (!IsRequired(clrParam))
                continue;

            // 🔥 CAST correcto al tipo concreto
            if (opParam is OpenApiParameter concrete)
            {
                concrete.Required = true;
            }
        }
    }

    private static bool IsRequired(ParameterInfo param)
    {
        var type = param.ParameterType;

        if (type == typeof(string))
            return !IsNullableReference(param);

        if (type.IsValueType)
            return Nullable.GetUnderlyingType(type) == null;

        return !IsNullableReference(param);
    }

    private static bool IsNullableReference(ParameterInfo param)
    {
        var ctx = new NullabilityInfoContext();
        return ctx.Create(param).WriteState == NullabilityState.Nullable;
    }
}