using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Benday.WebSecurity
{
    public class ClaimAuthorizationHandler :
        AuthorizationHandler<ClaimAuthorizationRequirement>
    {
        private readonly IRouteDataAccessor _routeDataAccessor;

        public ClaimAuthorizationHandler(IRouteDataAccessor routeDataAccessor)
        {
            _routeDataAccessor = routeDataAccessor;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ClaimAuthorizationRequirement requirement)
        {
            if (requirement == null ||
                requirement.Roles == null ||
                requirement.PermissionNames == null ||
                (requirement.Roles.Count == 0 && requirement.PermissionNames.Count == 0))
            {
                context.Fail();
            }

            var id = _routeDataAccessor.GetId();

            if (string.IsNullOrEmpty(id) == true)
            {
                context.Fail();
            }
            else
            {
                var utility = new SecurityUtility(
                    context.User.Identity, context.User);

                var isAuthorized = CheckIsAuthorized(requirement, id, utility);

                if (isAuthorized == true)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }

            return Task.CompletedTask;
        }

        private static bool CheckIsAuthorized(
            ClaimAuthorizationRequirement requirement,
            string id, SecurityUtility utility)
        {
            var isAuthorized = false;

            if (isAuthorized == false)
            {
                foreach (var role in requirement.Roles)
                {
                    if (utility.IsInRole(role) == true)
                    {
                        isAuthorized = true;
                        break;
                    }
                }
            }

            if (isAuthorized == false)
            {
                foreach (var permissionName in requirement.PermissionNames)
                {
                    if (utility.IsAuthorized(permissionName, id, false) == true)
                    {
                        isAuthorized = true;
                        break;
                    }
                }
            }

            return isAuthorized;
        }
    }
}
