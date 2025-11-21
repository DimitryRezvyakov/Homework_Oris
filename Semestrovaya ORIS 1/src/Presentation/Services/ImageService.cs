using Presentation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Presentation.Services
{
    public class ImageService : IImageUploader
    {
        public ImageService() { }

        public bool TryConvertDataUrlToJpg(string base64, string fileName, string mimeType)
        {
            if (mimeType != "image/jpeg")
            {
                throw new NotSupportedException("Поддерживаются только jpeg");
            }

            string base64Data = base64;
            string outputPath = $"wwwroot/images/{fileName}";

            if (File.Exists(outputPath))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(base64Data))
            {
                Console.WriteLine("Base64 данные отсутствуют");
                return false;
            }

            byte[] imageBytes = Convert.FromBase64String(base64Data);
            File.WriteAllBytes(outputPath, imageBytes);

            // Проверяем, что файл создан
            if (!File.Exists(outputPath))
            {
                Console.WriteLine("Не удалось создать файл");
                return false;
            }

            return true;
        }

        public bool DeleteOldImages(string[] filePaths)
        {
            try
            {
                foreach (var filePath in filePaths)
                {
                    // Убираем домен из пути
                    var localPath = RemoveDomainFromPath(filePath);

                    if (File.Exists(localPath))
                    {
                        File.Delete(localPath);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string RemoveDomainFromPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return filePath;

            var prefix = "http://localhost:8888/";

            if (filePath.StartsWith(prefix))
            {
                return filePath.Substring(prefix.Length);
            }

            return filePath;
        }
    }
}