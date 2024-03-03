using Benday.CodeGenerator.Api;
using System;
using System.Security.Claims;

namespace Benday.WebSecurity;


public class AuthorizationUtility
{
    public static bool IsUserOrAdmin(IEnumerable<Claim> claims)
    {
        var exists = claims.Where(
            x => x.Type == ClaimTypes.Role &&
            (x.Value == ApiConstants.ClaimValue_Role_Admin ||
             x.Value == ApiConstants.ClaimValue_Role_User)
            ).Any();

        return exists;
    }
}
