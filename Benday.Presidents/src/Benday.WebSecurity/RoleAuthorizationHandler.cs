using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Benday.WebSecurity
{
    public class RoleAuthorizationHandler :
        AuthorizationHandler<RoleAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleAuthorizationRequirement requirement)
        {
            var utility = new SecurityUtility(context.User.Identity, context.User);

            if (utility.IsInRole(requirement.RoleName) == true)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
