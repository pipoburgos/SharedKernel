using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Builder;

namespace SharedKernel.Api.Middlewares
{
    public static class CurrentCultureMiddleware
    {
        public static IApplicationBuilder UseCurrentCulture(this IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                //get client prefered language
                var userLangs = context.Request.Headers["Accept-Language"].ToString();
                var firstLang = userLangs.Split(',').FirstOrDefault();

                //set allowed alanguage
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
