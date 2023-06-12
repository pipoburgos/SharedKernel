using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SharedKernel.Application.Documents;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace SharedKernel.Infrastructure.Documents.Excel.Npoi
{
    /// <summary>  </summary>
    public class NpoiExcelReader : DocumentReader, IExcelReader
    {
        /// <summary>  </summary>
        public override string Extension => "xlsx";

        /// <summary>  </summary>
        public override IEnumerable<T> Read<T>(Stream stream, Func<IRowData, int, T> cast)
        {
            using var workbook = new XSSFWorkbook(stream);

            var sheet = workbook.GetSheetAt(Configuration.SheetIndex);

            var columnNames = GetColumnNames(sheet);
            for (var rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null)
                    continue;

                yield return cast(new NpoiExcelRow(row.Cells, columnNames), rowIndex + 1);
            }
        }

        /// <summary>  </summary>
        public DataSet ReadTabs(Stream stream)
        {
            var dataSet = new DataSet();

            using IWorkbook workbook = new XSSFWorkbook(stream);

            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
                var sheet = workbook.GetSheetAt(i);
                var dataTable = ReadSheet(sheet);
                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }

        /// <summary>  </summary>
        public override DataTable Read(Stream stream)
        {
            using IWorkbook workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(Configuration.SheetIndex);
            return ReadSheet(sheet);
        }

        private DataTable ReadSheet(ISheet sheet)
        {
            var dataTable = new DataTable(sheet.SheetName);

            // write the header row
            var columnNames = GetColumnNames(sheet);

            if (Configuration.IncludeLineNumbers)
                dataTable.Columns.Add(Configuration.ColumnLineNumberName);

            foreach (var column in columnNames)
            {
                dataTable.Columns.Add(column);
            }

            // write the rest
            for (var i = 1; i < sheet.PhysicalNumberOfRows; i++)
            {
                var sheetRow = sheet.GetRow(i);
                var dtRow = dataTable.NewRow();

                var valores = new List<string>();
                if (Configuration.IncludeLineNumbers)
                    valores.Add((sheetRow?.RowNum + 1).ToString());

                dataTable.Columns
                    .Cast<DataColumn>()
                    .ToList()
                    .ForEach(c => valores.Add(sheetRow!.GetCell(c.Ordinal, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()));

                if (sheetRow == default || !valores.Skip(1).Any() || valores.Skip(1).All(string.IsNullOrWhiteSpace))
                    continue;

                dtRow.ItemArray = valores.Cast<object>().Take(dataTable.Columns.Count).ToArray();
                dataTable.Rows.Add(dtRow);
            }

            return dataTable;
        }

        private List<string> GetColumnNames(ISheet sheet)
        {
            var headerRow = sheet.GetRow(0);
            var duplicates = 0;
            var columns = new List<string>();
            foreach (var headerCell in headerRow.Select(h => h.ToString()).Where(h => !string.IsNullOrWhiteSpace(h)))
            {
                var headerCellTrim = headerCell.Trim();
                if (columns.Contains(headerCellTrim))
                {
                    duplicates++;
                    columns.Add($"{headerCellTrim}_{duplicates}");
                }
                else
                {
                    columns.Add(headerCellTrim);
                }
            }

            return columns;
        }
    }
}
