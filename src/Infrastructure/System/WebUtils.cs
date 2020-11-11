using System.Net;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System
{
    public class WebUtils : IWeb
    {
        public string HtmlEncode(string str)
        {
            return WebUtility.HtmlEncode(str);
        }

        public string HtmlDecode(string str)
        {
            return WebUtility.HtmlDecode(str);
        }

        public string UrlEncode(string str)
        {
            return WebUtility.UrlEncode(str);
        }

        public string UrlDecode(string str)
        {
            return WebUtility.UrlDecode(str);
        }
    }
}
