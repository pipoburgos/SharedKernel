using SharedKernel.Application.Extensions;
using System.Text.Json;

namespace SharedKernel.Infrastructure.NetJson.Policies;

/// <summary> . </summary>
public class PascalCaseNamingPolicy : JsonNamingPolicy
{
    /// <summary> . </summary>
    public override string ConvertName(string name)
    {
        return name.ToPascalCase();
    }
}
