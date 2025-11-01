using CustomMVC.App.DependencyInjection;
using CustomMVC.App.Hosting.Abstractions;
using CustomMVC.App.MVC.Controllers.Abstractions;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execute.Samples
{
    public class TestRequest : IRequest
    {
    }

    public class TestRequestHandler : IRequestHandler<TestRequest>
    {
        public async Task Handle(TestRequest request, CancellationToken cts)
        {
            Console.WriteLine("Empty");

            await Task.CompletedTask;
        }
    }

    public class TestRequestOnReturn : IRequest<string>
    {

    }

    public class TestRequestOnReturnHandler : IRequestHandler<TestRequestOnReturn, string>
    {
        public async Task<string> Handle(TestRequestOnReturn request, CancellationToken cts)
        {
            Console.WriteLine("String");
            return await Task.FromResult<string>("hello");
        }
    }

    public class Notification : INotification
    {

    }

    public class NotificationHandler : INotificationHandler<Notification>
    {
        private readonly IWebApplicationPipelineBuilder _pipeline;
        public NotificationHandler(IWebApplicationPipelineBuilder pipeline)
        {
            _pipeline = pipeline;
        }

        public async Task Handle(Notification notification, CancellationToken cts)
        {
            Console.WriteLine($"Pipeline: {_pipeline}");

            Console.WriteLine("Notification");

            await Task.CompletedTask;
        }
    }
}
