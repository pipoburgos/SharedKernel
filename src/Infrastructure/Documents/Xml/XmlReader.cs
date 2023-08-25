using SharedKernel.Application.Documents;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace SharedKernel.Infrastructure.Documents.Xml
{
    /// <summary>  </summary>
    public class XmlReader : DocumentReader, IXmlReader
    {
        /// <summary>  </summary>
        public override string Extension => "xml";

        /// <summary>  </summary>
        public override IEnumerable<IRowData> ReadStream(Stream stream)
        {
            var document = XElement.Load(stream);

            var lineNumber = 1;
            foreach (var node in document.Nodes())
            {
                lineNumber++;
                yield return new XmlRow(lineNumber, (node as XElement)!, Configuration.CultureInfo);
            }

            //var serializer = new XmlSerializer(typeof(T));

            //using var reader = new StringReader(stream);
            //yield return (T)serializer.Deserialize(reader);
        }

        /// <summary>  </summary>
        public override DataTable Read(Stream stream)
        {
            var dataTable = new DataTable();

            // Carga el archivo XML en un objeto XmlDocument
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);

            // Obtiene todos los elementos de nivel superior en el XML
            if (xmlDoc.DocumentElement == default)
                throw new ArgumentNullException(nameof(xmlDoc.DocumentElement));

            var xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

            // Obtiene las columnas de la tabla
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    if (!dataTable.Columns.Contains(childNode.Name))
                    {
                        // Crea una nueva columna en la tabla
                        dataTable.Columns.Add(childNode.Name, typeof(string));
                    }
                }
            }

            // Agrega las filas a la tabla
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                var dataRow = dataTable.NewRow();

                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    // Asigna el valor del nodo hijo a la columna correspondiente en la fila
                    dataRow[childNode.Name] = childNode.InnerText;
                }

                // Agrega la fila a la tabla
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}
