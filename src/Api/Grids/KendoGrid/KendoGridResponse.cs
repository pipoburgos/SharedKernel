using System.Collections.Generic;

namespace SharedKernel.Api.Grids.KendoGrid
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KendoGridResponse<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="total"></param>
        public KendoGridResponse(IEnumerable<T> data, int total)
        {
            Total = total;
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Data { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Total { get; }
    }
}
