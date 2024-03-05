using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Benday.Presidents.Api;
using Microsoft.AspNetCore.Authorization;

namespace Benday.WebSecurity
{
    public class LoggedInUsingEasyAuthHandler : AuthorizationHandler<LoggedInUsingEasyAuthRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LoggedInUsingEasyAuthRequirement requirement)
        {
            var identityProviderClaim =
                FindClaim(context, SecurityConstants.Claim_X_MsClientPrincipalIdp);

            if (identityProviderClaim == null)
            {
                // not logged in
                context.Fail();
            }
            else
            {
                if (AuthorizationUtility.IsUserOrAdmin(context.User.Claims) == true)
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

        private static Claim FindClaim(AuthorizationHandlerContext context, string claimName)
        {
            var match = context.User.Claims.Where(
                x => x.Type == claimName
                ).FirstOrDefault();

            return match;
        }
    }
}
