using CustomMVC.App.Core.Http.HttpMethods.Attributes;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding.Attributes;
using CustomMVC.App.MVC.Controllers.Results;
using Execute.Samples;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.Test.Samples
{
    public class IndexPostModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class HomeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _mediator.Publish(new Notification(), new CancellationToken());
            return Ok("Hello");
        }

        [HttpPost]
        public IActionResult IndexPost(IndexPostModel model)
        {
            return View(new
            {
                user = new
                {
                    Name = model.Name,
                    Password = model.Password
                }
            });
        }
    }
}
