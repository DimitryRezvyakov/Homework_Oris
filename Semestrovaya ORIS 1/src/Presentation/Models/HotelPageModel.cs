using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class HotelPageModel
    {
        public int ParentCount { get; set; }
        public int ChildCount { get; set; }
        public string StartDate { get; set; } = "Завтра";
        public string Nutrition { get; set; } = "Без питания";
        public int NightsCount { get; set; }
        public string HotelCountry { get; set; } = "Не определено";
        public string HotelResort { get; set; } = "Не определено";
        public string HotelName { get; set; } = "default";
        public object[] HotelStars { get; set; } = new object[0];
        public decimal HotelRating { get; set; }
        public string? HotelType { get; set; }
        public string HotelDescription { get; set; } = "default";
        public string HotelHtmlDescription { get; set; } = "default";
        public decimal Cost { get; set; }
        public List<string> Images { get; set; }
    }
}
