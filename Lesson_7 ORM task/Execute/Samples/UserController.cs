using CustomMVC.App.Common.Abstractions;
using CustomMVC.App.Core.Http.HttpMethods.Attributes;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding.Attributes;
using CustomMVC.App.MVC.Controllers.Results;
using MyORMLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execute.Samples
{
    public class UserController : ControllerBase
    {
        private readonly IORMContext _context;
        public UserController(IORMContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.ReadByAll<UserModel>();

            return Ok(users);
        }

        [HttpGet]
        public IActionResult GetById([FromQuery] int id)
        {
            var user = _context.ReadById<UserModel>(id);

            return Ok(user);
        }

        [HttpPost]
        public IActionResult PostUser(UserModel user)
        {
            _context.Create(user);

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdatePost(UserModel user, [FromQuery] int id)
        {
            _context.Update(id, user);

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeletePost([FromQuery] int id)
        {
            _context.Delete<UserModel>(id);

            return Ok();
        }
    }

    public class UserModel()
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
