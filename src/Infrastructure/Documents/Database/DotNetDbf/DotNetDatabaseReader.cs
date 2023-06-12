using DotNetDBF;
using SharedKernel.Application.Documents;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace SharedKernel.Infrastructure.Documents.Database.DotNetDbf
{
    /// <summary>  </summary>
    public class DotNetDatabaseReader : DocumentReader, IDatabaseReader
    {
        /// <summary>  </summary>
        public override string Extension => "dbf";

        /// <summary>  </summary>
        public override IEnumerable<T> Read<T>(Stream stream, Func<IRowData, int, T> cast)
        {
            using var reader = new DBFReader(stream);

            reader.SetSelectFields(reader.Fields.Select(f => f.Name).ToArray());

            for (var row = 0; row < reader.RecordCount; row++)
            {
                var rowData = new DatabaseRow(reader.NextRecord().ToList(), reader.Fields.Select(x => x.Name).ToList(),
                    Configuration.CultureInfo);
                yield return cast(rowData, row);
            }
        }

        /// <summary>  </summary>
        public override DataTable Read(Stream stream)
        {
            using var reader = new DBFReader(stream);

            var dataTable = new DataTable();
            if (Configuration.IncludeLineNumbers)
                dataTable.Columns.Add(Configuration.ColumnLineNumberName, typeof(int));

            foreach (var header in reader.Fields)
            {
                dataTable.Columns.Add(header.Name);
            }

            reader.SetSelectFields(reader.Fields.Select(f => f.Name).ToArray());

            for (var row = 0; row < reader.RecordCount; row++)
            {
                var dtRow = dataTable.NewRow();

                var values = new List<object>();

                if (Configuration.IncludeLineNumbers)
                    values.Add(row + 1);

                values.AddRange(reader.NextRecord().Select(x => x).ToList());

                dtRow.ItemArray = values.Take(dataTable.Columns.Count).ToArray();
                dataTable.Rows.Add(dtRow);
            }

            return dataTable;
        }
    }
}
