namespace SharedKernel.Domain.Entities.Paged;

/// <summary> The filter operator (comparison). </summary>
public enum Operator
{
    /// <summary> . </summary>
    EqualTo = 1,

    /// <summary> . </summary>
    NotEqualTo = 2,

    /// <summary> . </summary>
    IsEqualToNull = 3,

    /// <summary> . </summary>
    IsNotEqualToNull = 4,

    /// <summary> . </summary>
    LessThan = 5,

    /// <summary> . </summary>
    LessThanOrEqualTo = 6,

    /// <summary> . </summary>
    GreaterThan = 7,

    /// <summary> . </summary>
    GreaterThanOrEqualTo = 8,

    /// <summary> . </summary>
    StartsWith = 9,

    /// <summary> . </summary>
    NotStartsWith = 10,

    /// <summary> . </summary>
    EndsWith = 11,

    /// <summary> . </summary>
    NotEndsWith = 12,

    /// <summary> . </summary>
    Contains = 13,

    /// <summary> . </summary>
    NotContains = 14,

    /// <summary> . </summary>
    IsEmpty = 15,

    /// <summary> . </summary>
    IsNotEmpty = 16,

    /// <summary> . </summary>
    DateEqual = 17,

    /// <summary> . </summary>
    NotDateEqual = 18
}