using SharedKernel.Application.Documents;
using System.Data;

namespace SharedKernel.Infrastructure.Documents.Txt;

/// <summary> </summary>
public class TxtReader : DocumentReader, ITxtReader
{
    /// <summary> </summary>
    public override string Extension => "txt";

    /// <summary> </summary>
    public override IEnumerable<IRowData> ReadStream(Stream stream)
    {
        using var streamReader = new StreamReader(stream);
        ColumnNames = streamReader.ReadLine()!.Split(Configuration.Separator).Select(x => x.Trim()).ToList();

        var lineNumber = 1;
        while (!streamReader.EndOfStream)
        {
            var rows = streamReader.ReadLine()!.Split(Configuration.Separator).ToList();

            lineNumber++;
            yield return new TxtRow(lineNumber, rows, ColumnNames, Configuration.CultureInfo);
        }
    }

    /// <summary> </summary>
    public override DataTable Read(Stream stream)
    {
        using var streamReader = new StreamReader(stream);
        var dataTable = new DataTable();
        ColumnNames = streamReader.ReadLine()!.Split(Configuration.Separator).ToList();

        if (Configuration.IncludeLineNumbers)
            dataTable.Columns.Add(Configuration.ColumnLineNumberName, typeof(int));

        foreach (var header in ColumnNames)
        {
            dataTable.Columns.Add(header);
        }

        var lineNumber = 1;
        while (!streamReader.EndOfStream)
        {
            var rows = streamReader.ReadLine()!.Split(Configuration.Separator);
            var dr = dataTable.NewRow();
            if (Configuration.IncludeLineNumbers)
                dr[Configuration.ColumnLineNumberName] = lineNumber;

            lineNumber++;
            for (var i = 1; i < ColumnNames.Count; i++)
            {
                dr[i] = rows[i];
            }
            dataTable.Rows.Add(dr);
        }

        return dataTable;
    }
}
