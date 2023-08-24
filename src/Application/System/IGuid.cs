namespace SharedKernel.Application.System
{
    /// <summary>
    /// Random Guid generator
    /// </summary>
    public interface IGuid
    {
        /// <summary>
        /// Generate a random Guid
        /// </summary>
        /// <returns></returns>
        Guid NewGuid();

        /// <summary>
        /// Generate a Guid from int
        /// Sample: from number 15 generates 00000000-0000-0000-0000-000000000015
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Guid NewGuid(int value);
    }
}
