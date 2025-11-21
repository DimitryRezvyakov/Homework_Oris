using Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class UpdateHotelModel
    {
        public Hotel Hotel { get; set; }
        public int[] HotelTags { get; set; }
        public string[] ImagesToDelete { get; set; }
    }
}
