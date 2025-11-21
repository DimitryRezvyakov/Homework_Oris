using Application.Entities;
using Application.Features.Countries.Queries;
using Application.Features.Hotels.Queries;
using Application.Features.Resorts.Queries;
using Application.Features.Tags.Queries;
using CustomMVC.App.Core.Http.HttpMethods.Attributes;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding.Attributes;
using CustomMVC.App.MVC.Controllers.Results;
using Mediator.Interfaces;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var hotelsResult = await _mediator.Send(new GetAllHotelsMediumDTOQuery(), new CancellationToken());
            var tagsResult = await _mediator.Send(new GetAllTagsQuery(), new CancellationToken());

            if (hotelsResult.Succeeded && tagsResult.Succeeded)
            {
                List<HotelModel> hotels = new List<HotelModel>();

                foreach (var hotel in hotelsResult.Data!)
                {
                    hotels.Add(new HotelModel
                    {
                        Cost = hotel.Price.ToString(),
                        Id = hotel.Id.ToString(),
                        Name = hotel.Name,
                        Description = hotel.Description,
                        Location = hotel.Location,
                        Raiting = hotel.Raiting.ToString(),
                        Stars = new object[hotel.Stars],
                        Image = hotel.Image,
                    });
                }

                return View(new { Model = new IndexPageModel(hotels, tagsResult.Data!), Succeed = true });
            }

            else
            {
                return View(new {Model = new {HotelResult = hotelsResult, TagsResult = tagsResult}, Succeed = false});
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApplyFilters([FromBody] ApplyFiltersRequestModel filters)
        {
            if (filters != null)
            {
                var hotelsResult = await _mediator.Send(new GetAllHotelsByFilterQuery()
                {
                    Amenities = filters.Amenities,
                    Budget = filters.Budget,
                    DateRange = filters.DateRange,
                    Destination = filters.Destination,
                    Guests = filters.Guests,
                    Meal = filters.Meal,
                    MinRating = filters.MinRating,
                    MinStars = filters.MinStars
                }, new CancellationToken());

                List<HotelModel> hotels = new List<HotelModel>();

                if (!hotelsResult.Succeeded)
                    return View(new { Succeed = false });

                foreach (var hotel in hotelsResult.Data!)
                {
                    hotels.Add(new HotelModel
                    {
                        Cost = hotel.Price.ToString(),
                        Id = hotel.Id.ToString(),
                        Name = hotel.Name,
                        Description = hotel.Description,
                        Location = hotel.Location,
                        Raiting = hotel.Raiting.ToString(),
                        Stars = new object[hotel.Stars],
                        Image = hotel.Image,
                    });
                }

                return View(new { Model = new HotelsPartialModel(hotels), Succeed = true });
            }

            return View(new { Succeed = false });
        }

        [HttpGet]
        public async Task<IActionResult> Hotel([FromQuery] HotelRequestModel model)
        {
            var hotelResult = await _mediator.Send(new GetHotelByIdQuery(model.HotelId), new CancellationToken());

            if (!hotelResult.Succeeded)
                return NotFound();

            var hotel = hotelResult.Data as Hotel;

            var resort = await _mediator.Send(new GetResortByIdQuery(hotel!.ResortId), new CancellationToken());

            string resortName = resort?.Data?.Name ?? "default";
            string countryName = "default";

            if (resort != null && resort.Succeeded)
            {
                var country = await _mediator.Send(new GetCountryById(resort.Data!.CountryId!), new CancellationToken());

                countryName = country?.Data?.Name ?? "default";
            }

            return View(new {Model = new HotelPageModel()
            {
                ChildCount = model.ChildrenCount,
                Cost = hotel!.Price,
                HotelDescription = hotel!.Description,
                HotelHtmlDescription = hotel!.HtmlDescription,
                HotelName = hotel!.Name,
                HotelRating = hotel!.Raiting,
                HotelStars = new object[hotel.Stars],
                HotelType = hotel.HotelType ?? "Отель",
                NightsCount = model.NightsCount,
                Nutrition = model.Nutrition ?? "Без питания",
                ParentCount = model.ParentsCount,
                StartDate = model.StartDate ?? "Today",
                HotelCountry = countryName,
                HotelResort = resortName,
                Images = hotel.Images,
            } 
            });
        }
    }
}
