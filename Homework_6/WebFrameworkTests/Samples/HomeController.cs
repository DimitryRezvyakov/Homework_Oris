using CustomMVC.App.Core.Http.HttpMethods.Abstractions;
using CustomMVC.App.Core.Http.HttpMethods.Attributes;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using CustomMVC.App.MVC.Controllers.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFrameworkTests.Samples
{
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Hello");
        }

        [HttpPost]
        public IActionResult PostRequest(PostModel model)
        {
            if (model.Name != null)
            {
                return Ok($"{model.Name}, {model.Age}");
            }

            return NotFound();
        }
    }

    public class PostModel()
    {
        public string Name { get; set; }
        public required int Age { get; set; }
    }
}
