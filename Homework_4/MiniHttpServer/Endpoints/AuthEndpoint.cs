using MiniHttpServer.Core.Attributes;
using MiniHttpServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniHttpServer.Endpoints
{
    [Endpoint]
    public class AuthEndpoint
    {
        //GET ../Auth/
        [HttpGet]
        public string? LoginPage()
        {
            return "Public/Olara/olara.html";
        }

        //POST ../Auth/
        [HttpPost]
        public string? Login(string email, string password)
        {
            EmailService.SendEmail(email, "Test", password);

            return "Public/Olara/olara.html";
        }

        //POST ../Auth/SendEmail
        [HttpPost("SendEmail")]
        public void SendEmail(string to, string title, string message)
        {
            EmailService.SendEmail(to, title, message);
        }
    }
}
