namespace SharedKernel.Application.System
{
    /// <summary> Date time manager. </summary>
    public interface IDateTime
    {
        /// <summary> Get utc date time. </summary>
        DateTime UtcNow { get; }

        /// <summary> Max date time value. </summary>
        DateTime MaxValue { get; }

        /// <summary> Client date time value. </summary>
        DateTime ClientNow { get; }

        /// <summary> Client date time value. </summary>
        DateTime ConvertToClientDate(DateTime dateTime);
    }
}
