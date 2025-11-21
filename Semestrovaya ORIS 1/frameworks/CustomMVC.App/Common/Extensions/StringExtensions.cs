using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Common.Extensions
{
    public static class StringExtensions
    {
        public static string GetContentTypeByFileExtension(this string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".pdf":
                    return "application/pdf";
                case ".txt":
                    return "text/plain";
                case ".json":
                    return "application/json";
                case ".html":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return "application/javascript";
                default:
                    return "application/octet-stream";
            }
        }

        public static string RawControllerName(this string name)
        {
            return name.Replace("Controller", "").ToLower();
        }
    }
}
