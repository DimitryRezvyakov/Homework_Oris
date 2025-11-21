using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common.Entities
{
    public class ModelState
    {
        /// <summary>
        /// Action parameters
        /// </summary>
        public object[]? Parameters { get; set; }

        /// <summary>
        /// Defines binding is sucessful
        /// </summary>
        public bool IsValid { get; set; }

        public ModelState() { }

        private ModelState(object[]? parameters, bool isValid)
        {
            Parameters = parameters;
            IsValid = isValid;
        }

        /// <summary>
        /// Failed model binding
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static ModelState FromFailure(Exception? exception = null)
        {
            return new ModelState(null, false);
        }

        /// <summary>
        /// Succeed model binding
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ModelState FromSuccess(object[]? parameters = null)
        {
            return new ModelState(parameters, true);
        }
    }
}
