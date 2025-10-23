using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniHttpServer.Shared
{
    public static class PathHelper
    {
        public static string RemoveFilename(this string path)
        {
            string fileName = Path.GetFileName(path);

            // Проверяем, есть ли у файла расширение
            if (Path.HasExtension(fileName))
            {
                return Path.GetDirectoryName(path)?.Replace('\\', '/') ?? path;
            }

            return path.Replace('\\', '/');
        }
    }
}
