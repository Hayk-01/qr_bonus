using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using QRBonus.DAL.Models;
using QRBonus.DTO.ReportDtos;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;

namespace QRBonus.BLL.Helpers
{
    public static class ExcelExportHelper
    {
        public static byte[] PrepareExcel(DataTable table, bool isFilterNeeded = true)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("report");

                AddColumns(worksheet, table.Rows.Count, table.Columns, isFilterNeeded: isFilterNeeded);

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells["A2"].LoadFromDataTable(table, false);

                return package.GetAsByteArray();
            }
        }

        private static void AddColumns(ExcelWorksheet worksheet, int rowCount, DataColumnCollection data, string tableName = "name", bool isFilterNeeded = true)
        {
            rowCount = rowCount == 0 ? 2 : rowCount + 1;

            using (var rng = worksheet.Cells[1, 1, rowCount, data.Count])
            {
                var table = worksheet.Tables.Add(rng, tableName);

                var index = -1;

                foreach (var col in data)
                {
                    table.Columns[++index].Name = col.ToString();
                }

                table.ShowFilter = isFilterNeeded;
                table.ShowTotal = isFilterNeeded;
            }
        }

    }
}
