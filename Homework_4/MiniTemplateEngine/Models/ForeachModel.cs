using MiniTemplateEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTemplateEngineTests.Models
{
    public class ForeachModel : BaseModel
    {
        public string Content { get; set; } = String.Empty;
        public override int Start { get; set; }
        public override int Length { get; set; }
        public required IterationModel IterationModel { get; set; }
    }

    public class IterationModel
    {
        public string PropertyName { get; set; } = String.Empty;

        public string CollectionPath { get; set; } = String.Empty;
    }
}
