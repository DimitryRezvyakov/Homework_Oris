using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class HotelsPartialModel
    {
        public List<HotelModel> Hotels { get; set; } = new();

        public HotelsPartialModel(List<HotelModel> hotels)
        {
            Hotels = hotels;
        }
    }
}
