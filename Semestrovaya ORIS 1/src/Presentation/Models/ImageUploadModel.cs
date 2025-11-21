using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class ImageUploadModel
    {
        public string FileName { get; set; }
        public string FileData { get; set; }
        public string MimeType { get; set; }
    }
}
