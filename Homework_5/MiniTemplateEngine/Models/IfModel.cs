using MiniTemplateEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTemplateEngineTests.Models
{
    public class IfModel : BaseModel
    {
        public required Condition Condition { get; set; }
        public string IfContent { get; set; } = String.Empty;
        public string? ElseContent { get; set; }
        public override int Start { get; set; }
        public override int Length { get; set; }
    }

    public class Condition
    {
        public required string PropertyPath { get; set; }
    }
}
