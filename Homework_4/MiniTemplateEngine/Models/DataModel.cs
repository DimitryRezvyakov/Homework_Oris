using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTemplateEngine.Models
{
    public class DataModel : BaseModel
    {
        public string Content { get; set; } = String.Empty;
        public override int Start { get; set; }
        public override int Length {get; set;}
    }
}
