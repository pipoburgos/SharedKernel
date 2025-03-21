﻿using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SharedKernel.Application.Documents;
using System.Globalization;

namespace SharedKernel.Infrastructure.NPOI.Documents.Excel;

/// <summary> . </summary>
public class NpoiExcelWriter : IExcelWriter
{
    /// <summary> . </summary>
    public Stream Write<T>(IEnumerable<T> elements, Dictionary<string, string> headers, string sheetName)
    {
        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet(sheetName);
        SetHeaders(headers, sheet);
        SetValues(elements, sheet, headers.Keys.Select(e => e).ToList());
        //AutoSize(headers, sheet);
        return ToMemoryStream(workbook);
    }

    /// <summary> . </summary>
    private static Stream ToMemoryStream(XSSFWorkbook workbook)
    {
        var stream = new MemoryStream();
        workbook.Write(stream, true);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    private static void SetValues<T>(IEnumerable<T> elements, ISheet sheet, List<string> headers)
    {
        var rowIndex = 1;
        foreach (var data in elements)
        {
            var dataRow = sheet.CreateRow(rowIndex);
            var colIndex = 0;

            var values = headers
                .Select(header => typeof(T).GetProperties().Single(e => e.Name == header))
                .Select(prop => prop.GetValue(data, null));

            foreach (var value in values)
            {
                if (value == null)
                {

                    dataRow.CreateCell(colIndex);
                }
                else
                {

                    var cell = dataRow.CreateCell(colIndex);

                    var valueType = value!.GetType();

                    var setCellValueMethod = typeof(ICell).GetMethod("SetCellValue", [valueType]);

                    if (setCellValueMethod != null)
                    {
                        setCellValueMethod.Invoke(cell, [value]);
                    }
                    else
                    {
                        cell.SetCellValue(value.ToString());
                    }

                    if (valueType == typeof(DateTime))
                    {
                        var date = (DateTime)value;

                        var cellStyle = sheet.Workbook.CreateCellStyle();
                        var dateFormat = sheet.Workbook.CreateDataFormat();

                        var format = CultureInfo.CurrentCulture.DateTimeFormat;

                        cellStyle.DataFormat = dateFormat.GetFormat(date.TimeOfDay == TimeSpan.Zero
                            ? format.ShortDatePattern
                            : $"{format.ShortDatePattern} {format.ShortTimePattern}");

                        cell.CellStyle = cellStyle;
                    }
                }
                colIndex++;
            }

            rowIndex++;
        }
    }

    //private static void AutoSize(Dictionary<string, string> headers, ISheet sheet)
    //{
    //    for (int i = 1; i <= headers.Count; i++)
    //    {
    //        sheet.AutoSizeColumn(i);
    //        GC.Collect();
    //    }
    //}

    private static void SetHeaders(Dictionary<string, string> headers, ISheet sheet)
    {
        var row = sheet.CreateRow(0);
        var rowIndex = 0;
        headers.Values.ToList().ForEach(excelColumn =>
        {
            row.CreateCell(rowIndex).SetCellValue(excelColumn);
            rowIndex++;
        });
    }
}
