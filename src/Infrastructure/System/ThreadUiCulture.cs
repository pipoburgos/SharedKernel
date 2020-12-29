using System.Threading;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ThreadUiCulture : ICulture
    {
        /// <summary>
        /// 
        /// </summary>
        public string LanguageId => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

        /// <summary>
        /// 
        /// </summary>
        public string LanguageSpaId => "es";
    }
}
