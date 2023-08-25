using SharedKernel.Application.System;
using System.Net;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>
    /// 
    /// </summary>
    public class WebUtils : IWeb
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string HtmlEncode(string str)
        {
            return WebUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string HtmlDecode(string str)
        {
            return WebUtility.HtmlDecode(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string? UrlEncode(string str)
        {
            return WebUtility.UrlEncode(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string UrlDecode(string str)
        {
            return WebUtility.UrlDecode(str);
        }
    }
}
