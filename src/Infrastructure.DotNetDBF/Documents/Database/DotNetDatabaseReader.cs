using DotNetDBF;
using SharedKernel.Application.Documents;
using SharedKernel.Infrastructure.Documents;
using System.Data;

namespace SharedKernel.Infrastructure.DotNetDBF.Documents.Database;

/// <summary>  </summary>
public class DotNetDatabaseReader : DocumentReader, IDatabaseReader
{
    /// <summary>  </summary>
    public override string Extension => "dbf";

    /// <summary>  </summary>
    public override IEnumerable<IRowData> ReadStream(Stream stream)
    {
        using var reader = new DBFReader(stream);

        ColumnNames = reader.Fields.Select(f => f.Name).ToList();
        reader.SetSelectFields(ColumnNames.ToArray());

        for (var row = 0; row < reader.RecordCount; row++)
        {
            var rowData = new DatabaseRow(row, reader.NextRecord().ToList(), reader.Fields.Select(x => x.Name).ToList(), Configuration.CultureInfo);
            yield return rowData;
        }
    }

    /// <summary>  </summary>
    public override DataTable Read(Stream stream)
    {
        using var reader = new DBFReader(stream);

        var dataTable = new DataTable();
        if (Configuration.IncludeLineNumbers)
            dataTable.Columns.Add(Configuration.ColumnLineNumberName, typeof(int));

        ColumnNames = reader.Fields.Select(f => f.Name).ToList();

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