using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Services.Interfaces
{
    public interface IImageUploader
    {
        public bool TryConvertDataUrlToJpg(string dataUrl, string outputPath, string mimeType);
    }
}
