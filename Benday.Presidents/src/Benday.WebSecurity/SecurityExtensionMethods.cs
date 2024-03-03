using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace Benday.WebSecurity;


public static class SecurityExtensionMethods
{
    public static IApplicationBuilder UsePopulateClaimsMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PopulateClaimsMiddleware>();
    }

    public static Claim? GetClaim(
        this IEnumerable<Claim> claims, string claimName)
    {
        if (claims == null)
        {
            return null;
        }
        else
        {
            var match = (from temp in claims
                         where temp.Type == claimName
                         select temp).FirstOrDefault();

            return match;
        }
    }

    public static string GetClaimValue(
        this IEnumerable<Claim> claims, string claimName)
    {
        var match = claims.GetClaim(claimName);

        if (match == null)
        {
            return null;
        }
        else
        {
            return match.Value;
        }
    }

    public static int GetClaimValueAsInt(
        this IEnumerable<Claim> claims, string claimName)
    {
        var match = claims.GetClaim(claimName);

        if (match == null)
        {
            throw new InvalidOperationException($"Claim does not exist.");
        }
        else if (string.IsNullOrWhiteSpace(match.Value) == true)
        {
            throw new InvalidOperationException($"Claim value is not an int.");
        }
        else
        {
            if (int.TryParse(match.Value, out var temp) == true)
            {
                return temp;
            }
            else
            {
                throw new InvalidOperationException($"Claim value is not an int.");
            }
        }
    }

    public static bool ContainsClaimInt(
        this IEnumerable<Claim> claims, string claimName)
    {
        var match = claims.GetClaim(claimName);

        if (match == null || string.IsNullOrWhiteSpace(match.Value) == true)
        {
            return false;
        }
        else
        {
            if (int.TryParse(match.Value, out var _) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static bool ContainsClaim(
        this IEnumerable<Claim> claims, string claimName)
    {
        var match = claims.GetClaim(claimName);

        if (match == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool ContainsRoleClaim(
        this IEnumerable<Claim> claims, string roleName)
    {
        var match = claims.Where(
            x => x.Type == ClaimTypes.Role && x.Value == roleName).FirstOrDefault();

        if (match == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static KeyValuePair<string, StringValues> GetHeader(
        this IHeaderDictionary headers, string name)
    {
        if (headers == null)
        {
            return default;
        }
        else
        {
            var match =
                (from temp in headers
                 where temp.Key == name
                 select temp).FirstOrDefault();

            return match;
        }
    }

    public static string GetHeaderValue(
        this IHeaderDictionary headers, string name)
    {
        if (headers.ContainsKey(name) == false)
        {
            return null;
        }
        else
        {
            return headers[name];
        }
    }
}
