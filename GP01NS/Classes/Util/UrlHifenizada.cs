using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GP01NS.Classes.Util
{
    public class UrlHifenizada : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.RouteData.Values["controller"] = requestContext.RouteData.Values["controller"].ToString().Replace("-", "");
            requestContext.RouteData.Values["action"] = requestContext.RouteData.Values["action"].ToString().Replace("-", "");

            return base.GetHttpHandler(requestContext);
        }
    }
}