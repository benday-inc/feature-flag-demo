using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Benday.Presidents.UnitTests.Presentation
{
    public static class ControllerSecurityUtility
    {

        public static void SetControllerContext(
            Controller controller,
            string username,
            string[] roles)
        {
            var httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();

            httpContext.User = new GenericPrincipal(
                new GenericIdentity(username), roles);

            var controllerContext = new ControllerContext(
                new ActionContext(
                    httpContext, 
                    new RouteData(), 
                    new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()));

            controller.ControllerContext = controllerContext;
        }
    }
}
