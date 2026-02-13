using Microsoft.Extensions.Options;
using QRBonus.BLL.Models;
using QRBonus.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Helpers
{
    public class FileHelper
    {
        private readonly string _documentPath;
        private readonly FileSettings _fileSettings;
        private readonly string _rootPath;

        public FileHelper(IOptions<FileSettings> fileSettings)
        {
            _fileSettings = fileSettings.Value;
            _rootPath = _fileSettings.FilePath;
            _documentPath = _fileSettings.DocumentPath;
        }

        public async Task<string> UploadFile(FileDto file, string path, string id = null)
        {
            if (file.Data?.Length == 0 || file.Type?.Length == 0)
            {
                throw new ArgumentException();
            }

            path = Path.Combine(_rootPath, path);

            if (!string.IsNullOrEmpty(id))
            {
                path = Path.Combine(path, id);
            }

            var extension = "." + file.Type;

            CheckDirectoryExists(path);

            string filePath;

            filePath = Path.Combine(path, Guid.NewGuid().ToString("N") /*+ "." */ + extension);

            await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(file.Data));

            return ConvertToUrl(filePath);
        }

        public async Task<string[]> UploadFiles(FileDto[] files, string path, string id = null)
        {
            var lst = new string[files.Length];

            for (var i = 0; i < files.Length; i++)
            {
                lst[i] = await UploadFile(files[i], path, id);
            }

            return lst;
        }

        public bool DeleteFile(string path)
        {
            path = path.StartsWith("/") ? path.Substring(1) : path;
            var filePath = Path.Combine(_rootPath, path);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }


        private static void CheckDirectoryExists(string path)
        {
            var exists = Directory.Exists(path);

            if (!exists)
            {
                Directory.CreateDirectory(path);
            }
        }

        private string ConvertToUrl(string path)
        {
            return path.Replace(_rootPath, "").Replace('\\', '/');
        }

        public async Task<string> UploadDocument(FileDto file, string path, string name = null)
        {
            if (file.Data?.Length == 0 || file.Type?.Length == 0)
            {
                throw new ArgumentException();
            }

            path = Path.Combine(_documentPath, path);

            var extension = "." + file.Type;

            CheckDirectoryExists(path);

            string filePath;

            filePath = Path.Combine(path, name + extension);

            await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(file.Data));

            return ConvertToUrl(filePath);
        }
    }
}
