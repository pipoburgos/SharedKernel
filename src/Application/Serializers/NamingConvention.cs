namespace SharedKernel.Application.Serializers;

/// <summary> . </summary>
public enum NamingConvention
{
    /// <summary> No convert. </summary>
    NoAction = 0,

    /// <summary> camelCase </summary>
    CamelCase = 1,

    /// <summary> PascalCase </summary>
    PascalCase = 2,

    /// <summary> snake_case </summary>
    SnakeCase = 3,

    /// <summary> Train-Case  </summary>
    TrainCase = 4,

    /// <summary> kebap-case  </summary>
    KebapCase = 5,
}
