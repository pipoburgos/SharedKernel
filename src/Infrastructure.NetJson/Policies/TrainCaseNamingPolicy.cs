using SharedKernel.Application.Extensions;
using System.Text.Json;

namespace SharedKernel.Infrastructure.NetJson.Policies;

/// <summary> . </summary>
public class TrainCaseNamingPolicy : JsonNamingPolicy
{
    /// <summary> . </summary>
    public override string ConvertName(string name)
    {
        return name.ToTrainCase();
    }
}
