using CustomMVC.App.Common.Exceptions;
using CustomMVC.App.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Routing.Common
{
    public class RoutePattern
    {
        /// <summary>
        /// RoutePattern name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// RoutePattern pattern 
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// RoutePattern defaults
        /// </summary>
        public Defaults? Defaults {  get; set; }

        /// <summary>
        /// RoutePattern template
        /// </summary>
        public RouteTemplate RouteTemplate { get; set; }

        public RoutePattern(string name, string pattern, Defaults? defaults = null)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(pattern);

            //if we cant define a controller throw excepyion
            if ((pattern == "" || pattern == "/") && defaults == null)
            {
                throw new UnresolvableRouteException();
            }

            Pattern = pattern;
            Name = name;
            Defaults = defaults;
            RouteTemplate = new RouteTemplate(pattern);
        }
    }

    public class RouteTemplate
    {
        /// <summary>
        /// Controller name for pattern
        /// </summary>
        public string? ControllerName { get; set; }

        /// <summary>
        /// Controller name for action
        /// </summary>
        public string? ActionName { get; set; }

        /// <summary>
        /// All pattern segments
        /// </summary>
        public List<RouteTemplateSegment> Segments { get; } = new();

        public RouteTemplate(string pattern)
        {
            ParsePattern(pattern);
        }

        private void ParsePattern(string pattern)
        {
            var segments = pattern.Split("/", StringSplitOptions.RemoveEmptyEntries);

            foreach (var segment in segments)
            {
                //if segment like {controller=home}
                if (Regex.Match(segment, @"{*=*}").Success)
                {
                    var keyValue = segment.Trim('{').Trim('}').Split('=');

                    var key = keyValue[0];

                    var value = keyValue[1];

                    switch (key)
                    {
                        case "controller":
                            ControllerName = value;
                            break;
                        case "action":
                            ActionName = value;
                            break;
                        default:
                            throw new UnresolvableRouteException();
                    }

                    Segments.Add(new RouteTemplateSegment(value, false, key));
                }

                //if segment like {id?}
                else if (Regex.Match(segment, @"{*}").Success)
                {
                    var value = segment.Trim('{').Trim('}');

                    var isOptional = value.EndsWith("?");

                    Segments.Add(new RouteTemplateSegment(value.Trim('?'), isOptional, isPathParameter: true));
                }

                //if segment is a static string
                else if (Regex.Match(segment, @"[a-zA-z1234567890]").Success)
                {
                    Segments.Add(new RouteTemplateSegment(segment, false));
                }
                else
                {
                    throw new InvalidPatternException();
                }
            }
        }
    }

    public class RouteTemplateSegment
    {
        /// <summary>
        /// Segment name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Defines a segment is optional
        /// </summary>
        public bool isOptional { get; set; }

        /// <summary>
        /// Defines a segment  is path parameter
        /// </summary>
        public bool isPathParameter { get; set; }

        /// <summary>
        /// Segment default value
        /// </summary>
        public string? DefaultValue { get; set; }

        public RouteTemplateSegment(string name, bool Optional, string defaultValue = null, bool isPathParameter = false)
        {
            Name = name;
            isOptional = Optional;
            DefaultValue = defaultValue;
            this.isPathParameter = isPathParameter;
        }
    }

    public class Defaults
    {
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
    }
}
