using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace QRBonus.BLL.Helpers
{
    public static class CsvExportHelper
    {
        public static async Task<byte[]> ExportAsync(Dictionary<string, string> qrCodes)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                Encoding = Encoding.ASCII
            };

            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer, csvConfig))
            {
                foreach (var qr in qrCodes)
                {
                    csvWriter.WriteField(qr.Key);
                    csvWriter.WriteField(qr.Value);
                    csvWriter.NextRecord();
                }

                writer.Flush();
                return mem.ToArray();
            }
        }
        public static async Task<byte[]> ExportAsync(List<string> qrCodes)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                Encoding = Encoding.ASCII
            };

            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer, csvConfig))
            {
                foreach (var qr in qrCodes)
                {
                    csvWriter.WriteField(qr);
                    csvWriter.NextRecord();
                }

                writer.Flush();
                return mem.ToArray();
            }
        }

        public static string MakeTextForCsv(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.Replace("\"", "\"\""); ;

            return text.Contains(",") ? $"\"{text}\"" : text;
        }
    }
}
