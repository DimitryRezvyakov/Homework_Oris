using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public int ResortId { get; set; }
        public string Name { get; set; } = "default";
        public string? HotelType { get; set; } = "Отель";
        public decimal Price { get; set; }
        public int Stars { get; set; }
        public decimal Raiting { get; set; }
        public string Nutrition { get; set; } = "Без питания";
        public string Description { get; set; } = "default";
        public string HtmlDescription { get; set; } = "default";
        public List<string> Images { get; set; }
    }
}
