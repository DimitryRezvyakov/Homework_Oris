using Application.Features.Countries.Queries;
using Application.Features.Hotels.Queries;
using Application.Features.Resorts.Queries;
using Application.Features.Tags.Queries;
using CustomMVC.App.Core.Http.HttpMethods.Attributes;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding.Attributes;
using CustomMVC.App.MVC.Controllers.Results;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class HotelController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HotelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSearchData()
        {
            var countriesResult = await _mediator.Send(new GetAllCountries(), new CancellationToken());

            var resortsResult = await _mediator.Send(new GetAllResorts(), new CancellationToken());

            var hotelsResult = await _mediator.Send(new GetAllHotelsSmallDTOQuery(), new CancellationToken());

            if (countriesResult.Succeeded && resortsResult.Succeeded && hotelsResult.Succeeded)
            {
                return Ok(
                    new
                    {
                        countries = countriesResult.Data!.Select(c => new
                        {
                            id = c.Id,
                            name = c.Name,
                            imageSrc = c.IconUri,
                        }).ToArray(),
                        resorts = resortsResult.Data!.Select(r => new
                        {
                            id = r.Id,
                            name = r.Name,
                            countryId = r.CountryId,
                        }).ToArray(),
                        hotels = hotelsResult.Data!.Select(h => new
                        {
                            id = h.Id,
                            name = h.Name,
                            resortId = h.ResortId,
                        })
                    });
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllResorts()
        {
            var result = await _mediator.Send(new GetAllResorts(), new CancellationToken());

            if (result.Succeeded)
                return Ok(result.Data!.Select(r => new {resortName = r.Name, resortId = r.Id}));

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var result = await _mediator.Send(new GetAllTagsQuery(), new CancellationToken());

            if (result.Succeeded)
            {
                return Ok(result.Data!.Select(t => new { tagName = t.Name, tagId = t.Id }));
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelById([FromQuery] int id)
        {
            if (id == 0)
                return NotFound();

            var result = await _mediator.Send(new GetHotelByIdQuery(id), new CancellationToken());

            if (result.Succeeded)
                return Ok(result.Data);

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelTags([FromQuery] int id)
        {
            if (id == 0)
                return NotFound();

            var result = await _mediator.Send(new GetAllTagsByHotelIdQuery(id), new CancellationToken());

            if (result.Succeeded)
                return Ok(result.Data);

            return NotFound();
        }
    }
}
