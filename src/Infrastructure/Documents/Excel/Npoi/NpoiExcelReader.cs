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
    public class NpoiExcelReader : IExcelReader
    {
        /// <summary>  </summary>
        public string Extension => "xlsx";

        /// <summary>  </summary>
        public IEnumerable<T> Read<T>(Stream stream, Func<IRowData, int, T> cast)
        {
            return Read(stream, cast, 0);
        }

        /// <summary>  </summary>
        public IEnumerable<T> Read<T>(Stream stream, Func<IRowData, int, T> cast, int sheetIndex)
        {
            using var workbook = new XSSFWorkbook(stream);

            var sheet = workbook.GetSheetAt(sheetIndex);

            var columnNames = GetColumnNames(sheet);
            for (var rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null)
                    continue;

                yield return cast(new NpoiExcelRow(row.Cells, columnNames), rowIndex + 1);
            }
        }

        /// <summary>  </summary>
        public DataSet ReadTabs(Stream stream, bool includeLineNumbers = true)
        {
            var dataSet = new DataSet();

            using IWorkbook workbook = new XSSFWorkbook(stream);

            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
                var sheet = workbook.GetSheetAt(i);
                var dataTable = ReadSheet(sheet, includeLineNumbers);
                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }

        /// <summary>  </summary>
        public DataTable Read(Stream stream, bool includeLineNumbers = true)
        {
            return Read(stream, includeLineNumbers, 0);
        }

        /// <summary>  </summary>
        public DataTable Read(Stream stream, bool includeLineNumbers, int sheetIndex)
        {
            using IWorkbook workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(sheetIndex);
            return ReadSheet(sheet, includeLineNumbers);
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
