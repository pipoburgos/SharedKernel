using System.Threading;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    public class ThreadUiCulture : ICulture
    {
        public string LanguageId => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        public string LanguageSpaId => "es";
    }
}
