using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BilligKwhWebApp.Middleware
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        /// <summary>
        ///  Used to specify the locations that the view engine should search to locate views.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewLocations"></param>
        /// <returns></returns>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            // {2} is area, {1} is controller, {0} is the action
            string[] locations = new string[]
            {
                // Defaults
                "/Views/{1}/{0}.cshtml",
                "/Views/Shared/{0}.cshtml",

                // Generic
                "/Controllers/{1}/{0}.cshtml",
                "/Controllers/{1}/Views/{0}.cshtml",
                
                // Controller Specific
                "/Controllers/Developer/Views/{0}.cshtml",
            };
            // Add mvc default locations after ours
            return locations.Union(viewLocations);          
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.Values["customviewlocation"] = nameof(ViewLocationExpander);
        }
    }
}
