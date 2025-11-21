using Application.Entities;
using Application.Features.Hotels.Commands;
using Application.Features.Hotels.Queries;
using CustomMVC.App.Core.Http.HttpMethods.Attributes;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding.Attributes;
using CustomMVC.App.MVC.Controllers.Results;
using Mediator.Interfaces;
using Presentation.Models;
using Presentation.Services;
using Presentation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebFramework.MVC.Controllers.Abstractions.Attributes;

namespace Presentation.Controllers
{
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IImageUploader _imageUploader;

        public AdminController(IMediator mediator, IImageUploader imageUploader)
        {
            _mediator = mediator;
            _imageUploader = imageUploader;
        }

        [AllowAnonymous]
        public IActionResult RegisterPage()
        {
            return View(null);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromQuery]string name, [FromQuery]string password)
        {
            if (name != "Dima" || password != "24092006")
            {
                return NotFound();
            }

            var cookieToadd = new Cookie("isAuthorized", "some-key");
            cookieToadd.Expires = DateTime.Now.AddHours(6);

            Context.Response.Cookies.Add(cookieToadd);

            return Ok();
        }

        public IActionResult AdminIndex()
        {
            return View(null);
        }

        public IActionResult AdminCreateHotelPartial()
        {
            return View(null);
        }

        public IActionResult AdminUpdateHotelPartial()
        {
            return View(null);
        }

        public async Task<IActionResult> GetHotel([FromQuery] int id)
        {
            var hotelResult = await _mediator.Send(new GetHotelByIdQuery(id), new CancellationToken());

            if (hotelResult.Succeeded)
                return Ok(hotelResult.Data!);

            return Ok(hotelResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel(CreateHotelModel model)
        {
            var createResult = await _mediator.Send(new CreateHotelCommand(model.Hotel), new CancellationToken());

            if (!createResult.Succeeded)
                return NotFound();

            var createdHotel = await _mediator.Send(new GetHotelByNameQuery(model.Hotel.Name), new CancellationToken());

            var updateResult = await _mediator.Send(new UpdateHotelTagsCommand(createdHotel.Data!.Id!, model.HotelTags), new CancellationToken());

            return Ok(createResult);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateHotel(UpdateHotelModel model)
        {
            var updateHotelResult = await _mediator.Send(new UpdateHotelCommand(model.Hotel), new CancellationToken());

            if (updateHotelResult.Succeeded)
            {
                var deleteOldHotelTagsResult = await _mediator.Send(new DeleteAllHotelTagsByHotelIdCommand(model.Hotel.Id), new CancellationToken());

                var addNewTagsResult = await _mediator.Send(new UpdateHotelTagsCommand(model.Hotel.Id, model.HotelTags), new CancellationToken());

                var i = new ImageService();

                i.DeleteOldImages(model.ImagesToDelete);

                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult UploadHotelImage(ImageUploadModel model)
        {
           try
            {
                var i = new ImageService();
                i.TryConvertDataUrlToJpg(model.FileData, model.FileName, model.MimeType);

                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteHotel([FromQuery] int id)
        {
            var result = await _mediator.Send(new DeleteHotelByIdCommand(id), new CancellationToken());

            if (result.Succeeded)
                return Ok();

            return Ok(result);
        }
    }
}
