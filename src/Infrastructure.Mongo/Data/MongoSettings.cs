namespace SharedKernel.Infrastructure.Mongo.Data;

/// <summary> . </summary>
public class MongoSettings
{
    /// <summary> . </summary>
    public string? ConnectionString { get; set; }

    /// <summary> . </summary>
    public string? Database { get; set; }

    /// <summary> . </summary>
    public bool EnableTransactions { get; set; } = false;
}
