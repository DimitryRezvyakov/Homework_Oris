using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DestinationDto
    {
        public string Type { get; set; } = null!; // "country" или "resort"
        public int Id { get; set; }
    }
}
