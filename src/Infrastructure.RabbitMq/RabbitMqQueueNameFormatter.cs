using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.RabbitMq;

/// <summary> . </summary>
public static class RabbitMqQueueNameFormatter
{
    /// <summary> . </summary>
    public static string Format(IRequestType information)
    {
        return information.UniqueName;
    }

    /// <summary> . </summary>
    public static string FormatRetry(IRequestType information)
    {
        return $"retry.{Format(information)}";
    }

    /// <summary> . </summary>
    public static string FormatDeadLetter(IRequestType information)
    {
        return $"dead_letter.{Format(information)}";
    }
}
