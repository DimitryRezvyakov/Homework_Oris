using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class HotelMediumDTO
    {
        public int Id { get; set; }
        public int ResortId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public int Stars { get; set; }
        public decimal Raiting { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
