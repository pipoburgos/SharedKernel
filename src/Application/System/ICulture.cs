namespace SharedKernel.Application.System
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICulture
    {
        /// <summary>
        /// Current user culture
        /// </summary>
        string LanguageId { get; }

        /// <summary>
        /// Spanish default culture
        /// </summary>
        string LanguageSpaId { get; }
    }
}
