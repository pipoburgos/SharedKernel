using SharedKernel.Application.Documents;
using System;
using System.Linq;
using System.Xml.Linq;

namespace SharedKernel.Infrastructure.Documents.Xml
{
    /// <summary>  </summary>
    public class XmlRow : IRowData
    {
        private readonly XElement _element;

        /// <summary>  </summary>
        public XmlRow(XElement element)
        {
            _element = element;
        }

        /// <summary>  </summary>
        public T Get<T>(int index)
        {
            var cell = _element.Descendants().ToArray()[index];

            if (cell == default)
                return default;

            return (T)Convert.ChangeType(cell, typeof(T));
        }

        /// <summary>  </summary>
        public T Get<T>(string name)
        {
            var cell = _element.Descendants(name).SingleOrDefault();

            if (cell == default)
                return default;

            return (T)Convert.ChangeType(cell, typeof(T));
        }
    }
}
