using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class Resort
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public bool HasAirport { get; set; }
    }
}
