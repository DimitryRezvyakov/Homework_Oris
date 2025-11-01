using CustomMVC.App.Common;
using CustomMVC.App.Common.Exceptions;
using CustomMVC.App.Core.Abstractions;
using CustomMVC.App.Hosting.Application;
using CustomMVC.App.Hosting.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Routing.Extensions
{
    public static class RouteExtensions
    {
        private readonly static Logger<WebApplication> _logger = new();
        private readonly static UseRoutingOptions _useRoutingOptions = new();
        public static void UseRouting(this WebApplication app, Action<UseRoutingOptions>? opt = null)
        {
            if (opt != null)
            {
                opt(_useRoutingOptions);
            }

            app.Use(async (context, next) =>
            {
                _logger.LogInfo($"Mathing {context.Request.Uri?.AbsolutePath ?? "/"}, {_logger.type}");

                //Creating a matcher instance
                var matcher = Activator.CreateInstance(_useRoutingOptions.Matcher, new object[] { app.endpointDataSources }) as Matcher;

                //This exception can be handled by using DefaultExceptionHandler middleware
                ArgumentNullException.ThrowIfNull(matcher);

                //Trying to set endpoint
                try
                {
                    RouteEndpoint route = await matcher.MatchAsync(context);

                    _logger.LogInfo($"Sucsessfully matched {context.Request.Uri?.AbsolutePath ?? "/"}, {_logger.type}");

                    context.Endpoint = route;

                    await next();
                }

                //If endpoint was not found return 404 status code
                catch (RouteNotFoundException)
                {
                    context.Response.SetStatusCode(404);
                }

                //If other exception was occured return 500 status code, development only
                catch (Exception ex)
                {
                    _logger.LogFatal($"Matching exception", ex);

                    context.Response.SetStatusCode(500);
                }
            });
        }

        public static void UseEndpoints(this WebApplication app)
        {
            app.WebAppBuilder.PipeLine.EndpointHandler = async (context) =>
            {
                RouteEndpoint routeEnpoint = context.Endpoint;

                _logger.LogInfo($"Executing endpoint, {routeEnpoint}");

                var routeHandler = routeEnpoint.RequestDelegate;

                await routeHandler(context);
            };
        }
    }

    public class UseRoutingOptions()
    {
        /// <summary>
        /// Must inherit a Matcher abstract class
        /// </summary>
        private Type MatcherType { get; set; } = typeof(RouteMatcher);

        /// <summary>
        /// Gives access to a MatcherType
        /// </summary>
        public Type Matcher => MatcherType;

        /// <summary>
        /// Configure a custom matcher
        /// </summary>
        /// <typeparam name="T">Custom matcher type, must be inherit a Matcher abstract class</typeparam>
        public void SetMatcher<T>() where T : Matcher
        {
            MatcherType = typeof(T);
        }
    }
}
