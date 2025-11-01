using CustomMVC.App.Common;
using CustomMVC.App.Common.Exceptions;
using CustomMVC.App.Common.Extensions;
using CustomMVC.App.Core.Abstractions;
using CustomMVC.App.Core.Http;
using CustomMVC.App.Core.Http.HttpMethods.Abstractions;
using CustomMVC.App.Core.Routing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Routing
{
    public class RouteMatcher : Matcher
    {
        private readonly Logger<RouteMatcher> _logger = new();

        public RouteMatcher(List<EndpointDataSource> sources) : base(sources) { }

        public override async Task<RouteEndpoint> MatchAsync(HttpContext context)
        {
            _logger.LogInfo($"Trying to match {context.Request.Uri}");

            var httpMethod = context.Request.Method;
            var path = context.Request?.Uri?.AbsolutePath ?? "/";
            List<RouteEndpoint> endpoints = new();

            foreach ( var source in _sources )
            {
                foreach (var route in source.Endpoints)
                {
                    var routePattern = route.RoutePattern.RouteTemplate;

                    var pathTemplate = new RouteTemplate(path);

                    //Define that pattern and path is matching
                    bool isMath = IsMatch(pathTemplate, routePattern, context);

                    var routeMethod = route.Metadata.GetMetadata<IHttpMethodMetadata>();

                    //Defines that http methods is matching
                    bool? isMathMethods = null;

                    //if route doesn`t have a http method just skipping
                    if (routeMethod != null)
                        isMathMethods = httpMethod.Equals(routeMethod.Methods[0]);

                    if (isMath && (isMathMethods ?? true))
                        endpoints.Add(route);
                }
            }

            //if no endpoints throwing RouteNotFoundException
            if (endpoints.Count == 0)
                return await Task.FromException<RouteEndpoint>(new RouteNotFoundException());

            return await Task.FromResult(endpoints.First());
        }

        /// <summary>
        /// Defines that pattern and path is matching
        /// </summary>
        /// <param name="path">Request path</param>
        /// <param name="pattern">Route pattern</param>
        /// <param name="context">Http context for this request</param>
        /// <returns></returns>
        private static bool IsMatch(RouteTemplate path, RouteTemplate pattern, HttpContext context)
        {
            foreach (var (pathSegment, patternSegment) in path.Segments.ZipLongest(pattern.Segments))
            {
                //if both are null returning true
                if (pathSegment == null && patternSegment == null)
                    return true;

                // if pattern is ended and path is not returning false
                else if (patternSegment == null && pathSegment != null)
                    return false;

                //if path is ended and pattern segment is not optional return false
                else if (pathSegment == null && !patternSegment.isOptional)
                    return false;

                else
                {
                    if ((pathSegment.Name != patternSegment.Name) && !pathSegment.isPathParameter) 
                        return false;

                    else if ((pathSegment.Name != patternSegment.Name) && pathSegment.isPathParameter) 
                        context.RouteParametrs.Add(patternSegment.Name, pathSegment.Name);

                    continue;
                }
            }

            return true;
        }
    }
}
