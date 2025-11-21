using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    using Application.DTOs;
    using System;
    using System.Collections.Generic;

    public class ApplyFiltersRequestModel
    {
        public DestinationDto? Destination { get; set; }
        public DateRangeDto? DateRange { get; set; }
        public BudgetDto? Budget { get; set; }
        public GuestsDto? Guests { get; set; }
        public string? Meal { get; set; }          // null = "Любое"
        public decimal? MinRating { get; set; }    // null = не фильтровать
        public int? MinStars { get; set; }         // null или 0 = не фильтровать
        public List<int>? Amenities { get; set; }  // null или пустой = не фильтровать
    }
}
