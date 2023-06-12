using SharedKernel.Application.Documents;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace SharedKernel.Infrastructure.Documents.Csv
{
    internal class CsvReader : ICsvReader
    {
        public string Extension => "csv";

        public IEnumerable<T> Read<T>(Stream stream, Func<IRowData, int, T> cast)
        {
            var streamReader = new StreamReader(stream);
            var headers = streamReader.ReadLine()!.Split(';').ToList();

            var lineNumber = 1;
            while (!streamReader.EndOfStream)
            {
                var rows = streamReader.ReadLine()!.Split(';').ToList();

                lineNumber++;
                yield return cast(new CsvRow(rows, headers), lineNumber);
            }
        }

        public DataTable Read(Stream stream, bool includeLineNumbers = true)
        {
            var streamReader = new StreamReader(stream);
            var dataTable = new DataTable();
            var headers = streamReader.ReadLine()!.Split(';');

            if (includeLineNumbers)
                dataTable.Columns.Add("LineNumber", typeof(int));
            foreach (var header in headers)
            {
                dataTable.Columns.Add(header);
            }

            var numeroLinea = 1;
            while (!streamReader.EndOfStream)
            {
                var rows = streamReader.ReadLine()!.Split(';');
                var dr = dataTable.NewRow();
                if (includeLineNumbers)
                    dr["LineNumber"] = numeroLinea;

                numeroLinea++;
                for (var i = 1; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dataTable.Rows.Add(dr);
            }

            return dataTable;
        }
    }
}
