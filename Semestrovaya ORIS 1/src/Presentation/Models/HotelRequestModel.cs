using Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class HotelRequestModel
    {
        public int ParentsCount { get; set; }
        public int ChildrenCount { get; set; }
        public int HotelId {  get; set; }
        public string? StartDate { get; set; }
        public int NightsCount { get; set; }
        public string Nutrition { get; set; } = "Любое";
    }
}
