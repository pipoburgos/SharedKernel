namespace SharedKernel.Infrastructure.Data.Dapper.Queries
{
    /// <summary> </summary>
    public enum QueryConditions
    {
        /// <summary> </summary>
        Like = 1,

        /// <summary> </summary>
        Equals = 2,

        /// <summary> </summary>
        In = 3,

        /// <summary> </summary>
        Between = 4,

        /// <summary> </summary>
        HigherOrEquals = 5,

        /// <summary> </summary>
        LowerOrEquals = 6,

        /// <summary> </summary>
        EqualsOr = 7,

        /// <summary> </summary>
        LikeAnd = 8,

        /// <summary> </summary>
        HigherEqualsAndOr = 9,

        /// <summary> </summary>
        LowerEqualsAndOr = 10,

        /// <summary> </summary>
        IsNull = 11,

        /// <summary> </summary>
        DateEquals = 12
    }
}