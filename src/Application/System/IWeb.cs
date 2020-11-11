namespace SharedKernel.Application.System
{
    public interface IWeb
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string HtmlEncode(string str);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string HtmlDecode(string str);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string UrlEncode(string str);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string UrlDecode(string str);
    }
}
