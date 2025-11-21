using CustomMVC.App.Core.Middleware.Extensions;
using CustomMVC.App.Hosting.Application;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Common;
using CustomMVC.App.MVC.Extensions;
using System.Net;
using System.Threading;
using WebFrameworkTests.Samples.Mock;

namespace WebFrameworkTests
{
    [TestClass]
    public sealed class HttpListenerHostTests
    {

        public static string root = "http://localhost:8888/";
        private static Task? _appTask;
        private static CancellationTokenSource? _cancellationTokenSource;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _appTask = Task.Run(StartApplication);
        }

        [TestMethod]
        public async Task Start_OnValidRequestWithExistingRoute_ExpectOkJson()
        {
            //arrange
            var client = new HttpClient();

            //act
            HttpResponseMessage response = await client.GetAsync($"{root}Home");

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json", response.Content.Headers.ContentType?.MediaType);

        }

        [TestMethod]
        public async Task Start_OnValidRequestWithUnexistingRoute_Excpect404()
        {
            //arrange
            var client = new HttpClient();

            //act
            HttpResponseMessage response = await client.GetAsync($"{root}Unexist");

            //assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Start_OnStaticFileExcistRequest_ExcpectReturnsStaticFile()
        {
            //arrange
            var client = new HttpClient();

            //act
            HttpResponseMessage response = await client.GetAsync($"{root}index.html");

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("text/html", response.Content.Headers.ContentType?.MediaType);
            Assert.AreEqual("Hello", await response.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task Start_OnStaticFileUnexcistRequest_Excpect404()
        {
            //arrange
            var client = new HttpClient();

            //act
            HttpResponseMessage response = await client.GetAsync($"{root}unexcist.html");

            //assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Start_OnPostRequestWithValidData_ExceptOkJson()
        {
            var client = new HttpClient();

            var formData = new Dictionary<string, string>
            {
                { "Name", "John Doe" },
                { "Age", "30" }
            };

            var content = new FormUrlEncodedContent(formData);

            content.Headers.Remove("Content-Type");

            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            //act
            HttpResponseMessage response = await client.PostAsync($"{root}Home", content);

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json", response.Content.Headers.ContentType?.MediaType);
        }

        [TestMethod]
        public async Task Start_OnPostRequestWithUnvalidData_ExceptNotFoundJson()
        {
            var client = new HttpClient();

            var formData = new Dictionary<string, string>
            {

            };

            var content = new FormUrlEncodedContent(formData);

            content.Headers.Remove("Content-Type");

            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            //act
            HttpResponseMessage response = await client.PostAsync($"{root}Home", content);

            //assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

        private static void StartApplication()
        {
            var builder = WebApplication.CreateBuilder(opt =>
            {
                opt.ConfigureControllersProvider<ControllersProviderMock>();
                opt.ConfigureActionDescriptorsProvider<ActionDescriptorProviderMock>();
            });

            var app = builder.Build();

            app.UseDefaultExceptionHandler();

            app.UseStaticFiles();

            app.UseControllersWithViews();

            app.MapControllerRoute(
                "default",
                "{controller=Home}");

            app.Run();
        }
    }
}
