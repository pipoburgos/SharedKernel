using Microsoft.AspNetCore.Builder;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace SharedKernel.Api.Middlewares
{
    /// <summary>
    /// Current culture middleware
    /// </summary>
    public static class CurrentCultureMiddleware
    {
        /// <summary>
        /// Configure Accept-Language header with english, spanish and chinese cultures
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSharedKernelCurrentCulture(this IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                //get client prefered language
                var userLanguages = context.Request.Headers["Accept-Language"].ToString();
                var firstLang = userLanguages.Split(',').FirstOrDefault();

                //set allowed language
                var lang = firstLang switch
                {
                    "es" => "es-ES",
                    "en" => "en-US",
                    "zh" => "zh-CHS",
                    _ => "es-ES"
                };

                //switch culture
                Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                // Call the next delegate/middleware in the pipeline
                return next();
            });

            return app;
        }
    }
}
