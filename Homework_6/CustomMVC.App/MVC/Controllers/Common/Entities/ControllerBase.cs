using CustomMVC.App.Core.Http;
using CustomMVC.App.MVC.Controllers.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common.Entities
{
    /// <summary>
    /// Base class for all controllers
    /// </summary>
    public abstract class ControllerBase
    {
        /// <summary>
        /// Controller context
        /// </summary>
        public HttpContext Context { get; set; } = null!;

        /// <summary>
        /// Binding model state (Not working now)
        /// </summary>
        public ModelState ModelState { get; set; } = null!;

        public IActionResult Ok () => new Ok();

        public IActionResult Ok<T>(T data) => new Ok<T>(data);

        public IActionResult NotFound () => new NotFound();

        public IActionResult View(object? data) => new View(data);
    }
}
