using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GuestsDto
    {
        public int Adults { get; set; }
        public List<int>? ChildrenAges { get; set; } // возраст каждого ребёнка (0–15)
    }
}
