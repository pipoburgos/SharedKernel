using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace SharedKernel.Infrastructure.Documents
{
    /// <summary>  </summary>
    public class ExcelReader
    {
        /// <summary>  </summary>
        public IEnumerable<T> Read<T>(Stream stream, Func<IExcelRow, T> cast, int sheetIndex = 0)
        {
            var workbook = new XSSFWorkbook(stream);

            var sheet = workbook.GetSheetAt(sheetIndex);

            var columnNames = GetColumnNames(sheet);
            for (var rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null)
                    continue;

                yield return cast(new ExcelRow(row.Cells, columnNames));
            }
        }

        /// <summary>  </summary>
        public DataSet Read(Stream stream, bool includeLineNumbers = true)
        {
            var dataSet = new DataSet();

            IWorkbook workbook = new XSSFWorkbook(stream);

            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
                var sheet = workbook.GetSheetAt(i);
                var dataTable = ReadSheet(sheet, includeLineNumbers);
                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }

        private DataTable ReadSheet(ISheet sheet, bool includeLineNumbers = true)
        {
            var dataTable = new DataTable(sheet.SheetName);

            // write the header row
            var columnNames = GetColumnNames(sheet);

            if (includeLineNumbers)
                dataTable.Columns.Add("LineNumber");

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
                if (includeLineNumbers)
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
                if (columns.Contains(headerCell))
                {
                    duplicates++;
                    columns.Add($"{headerCell}_{duplicates}");
                }
                else
                {
                    columns.Add(headerCell);
                }
            }

            return columns;
        }
    }
}
