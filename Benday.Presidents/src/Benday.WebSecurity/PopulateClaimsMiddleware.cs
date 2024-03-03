using System.Security.Claims;
using System.Text.Json.Nodes;
using Benday.CodeGenerator.Api;
using Benday.CodeGenerator.Api.DomainModels;
using Benday.CodeGenerator.Api.ServiceLayers;
using Benday.JsonUtilities;

namespace Benday.WebSecurity;

public class PopulateClaimsMiddleware : IMiddleware
{
    private readonly ISecurityConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly ILogger<PopulateClaimsMiddleware> _logger;

    public PopulateClaimsMiddleware(ISecurityConfiguration configuration,
        IUserService userService,
        ILogger<PopulateClaimsMiddleware> logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), $"{nameof(configuration)} is null.");
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var claims = new List<Claim>();

        if (_configuration.DevelopmentMode == true)
        {
            ProcessDevelopmentModeClaims(context, claims);
        }
        else
        {
            ProcessNonDevelopmentModeClaims(context, claims);
        }

        if (_configuration.TestMode == true)
        {
            ValidateTestModeUser(context);
        }

        await next(context);
    }

    private void ValidateTestModeUser(HttpContext context)
    {
        if (context.User == null || context.User.Identity == null)
        {
            // continue;
        }
        else
        {
            var info = new UserInformation(
                new SimpleClaimsAccessor(context.User.Claims));

            var username = info.Username;

            if (string.IsNullOrWhiteSpace(username) == true ||
                _configuration.TestModeUsers.Contains(username) == false)
            {
                context.User = new ClaimsPrincipal();
            }
        }
    }

    private void ProcessNonDevelopmentModeClaims(
        HttpContext context, List<Claim> claims)
    {
        if (AddClaimsFromHeader(context, claims) == true)
        {
            AddClaimsFromAuthMeService(context, claims);
            AddClaimsFromDatabaseAndCreateUserIfNotPresent(context, claims, false);

            var identity = new ClaimsIdentity(claims, "EasyAuth");

            context.User = new ClaimsPrincipal(identity);
        }
    }

    private void ProcessDevelopmentModeClaims(
        HttpContext context, List<Claim> claims)
    {
        if (context.User != null &&
            context.User.Identity != null &&
            context.User.Identity.IsAuthenticated == true)
        {
            // copy the existing claims
            claims.AddRange(context.User.Claims);

            var info = new UserInformation(
                new SimpleClaimsAccessor(claims));

            var username = info.Username;

            username = username.Replace(".com", string.Empty)
                .Replace(".org", string.Empty);

            var tokens = username.Split("@");

            AddClaim(claims, ClaimTypes.GivenName, tokens.FirstOrDefault());
            AddClaim(claims, ClaimTypes.Surname, tokens.LastOrDefault());
            AddClaim(claims, ClaimTypes.Email, info.Username);
            AddClaim(claims, ClaimTypes.Name, info.Username);
            //AddClaim(claims,
            //    SecurityConstants.Claim_X_MsClientPrincipalName,
            //    info.EmailAddress);

            AddClaimsFromDatabaseAndCreateUserIfNotPresent(
                info, "DevelopmentMode", claims, true);

            var identity = new ClaimsIdentity(claims, "DevelopmentMode");

            context.User = new ClaimsPrincipal(identity);
        }
    }

    private void AddClaimsFromDatabaseAndCreateUserIfNotPresent(
        UserInformation info, 
        string identityProvider,
        List<Claim> claims, bool developmentModeAuth)
    {
        AddClaimsFromDatabaseAndCreateUserIfNotPresent(
            claims,
            identityProvider,
            info.Username,
            developmentModeAuth);
    }

    private void AddClaimsFromAuthMeService(
        HttpContext context, List<Claim> claims)
    {
        if (context.Request.Cookies.ContainsKey(SecurityConstants.Cookie_AppServiceAuthSession) == true)
        {
            var authMeJson = GetAuthMeInfo(context.Request);

            // var jsonArray = System.Text.Json.JsonDocument.Parse(authMeJson);

            // var jsonArray = JArray.Parse(authMeJson);
            var jsonArray = JsonNode.Parse(authMeJson).AsArray();

            if (jsonArray.Count == 0)
            {
                _logger.LogWarning($"AddClaimsFromAuthMeService(): No claims found in authme response. Authme response length: '{authMeJson.Length}'");
            }
            else
            {
                var editor = new JsonEditor(jsonArray[0].ToString(), true);

                AddClaimIfExists(claims, editor, ClaimTypes.Name);
                AddClaimIfExists(claims, editor, ClaimTypes.GivenName);
                AddClaimIfExists(claims, editor, ClaimTypes.Surname);
                if (AddClaimIfExists(claims, editor, ClaimTypes.Email) == false)
                {
                    var temp = editor.GetValue("user_id");

                    if (temp.IsNullOrWhitespace() == false)
                    {
                        claims.Add(new Claim(ClaimTypes.Email, temp));
                    }
                    else
                    {
                        temp = editor.GetValue("preferred_username");

                        if (temp.IsNullOrWhitespace() == false)
                        {
                            claims.Add(new Claim(ClaimTypes.Email, temp));
                        }
                    }
                }
            }
        }
    }

    private static bool AddClaimIfExists(List<Claim> claims, JsonEditor editor, string claimTypeName)
    {
        var temp = GetClaimValue(editor, claimTypeName);

        if (temp.IsNullOrWhitespace() == false)
        {
            AddClaim(claims, claimTypeName, temp);

            return true;
        }
        else
        {
            return false;
        }
    }

    private static void AddClaim(List<Claim> claims, string claimTypeName, string? value)
    {
        if (string.IsNullOrEmpty(value) == false &&
            claims.ContainsClaim(claimTypeName) == true)
        {
            var temp = claims.GetClaim(claimTypeName);

            if (temp != null)
            claims.Remove(temp);
        }

        claims.Add(new Claim(claimTypeName, value));
    }

    private static string GetClaimValue(JsonEditor editor, string claimName)
    {
        var args = new SiblingValueArguments
        {
            SiblingSearchKey = "typ",
            SiblingSearchValue = claimName,

            DesiredNodeKey = "val",
            PathArguments = new[] { "user_claims" }
        };

        var temp = editor.GetSiblingValue(args);

        return temp;
    }

    private static string GetAuthMeInfo(HttpRequest request)
    {
        var client = new AzureEasyAuthClient(request);

        if (client.IsReadyForAuthenticatedCall == false)
        {
            return null;
        }
        else
        {
            var resultAsString = client.GetUserInformationJson();

            return resultAsString;
        }
    }


    private void AddClaimsFromDatabaseAndCreateUserIfNotPresent(
        HttpContext context, List<Claim> claims, bool developmentModeAuth)
    {
        var identityProviderHeader =
            GetHeaderValue(context, SecurityConstants.Claim_X_MsClientPrincipalIdp);

        if (string.IsNullOrEmpty(identityProviderHeader) == true)
        {
            return;
        }

        var principalName =
            GetHeaderValue(
            context,
            SecurityConstants.Claim_X_MsClientPrincipalName);

        var username =
            $"{principalName}";

        if (string.IsNullOrEmpty(username) == true)
        {
            username = GetHeaderValue(
                context,
                SecurityConstants.Claim_X_MsClientPrincipalId);
        }

        if (identityProviderHeader != null &&
            username != null)
        {
            AddClaimsFromDatabaseAndCreateUserIfNotPresent(
                claims, identityProviderHeader, username, developmentModeAuth);
        }
    }

    private void AddClaimsFromDatabaseAndCreateUserIfNotPresent(
        List<Claim> claims, string identityProvider, string username, 
        bool developmentModeAuth)
    {
        var user = _userService.GetByUsername(username, identityProvider);

        if (user == null)
        {
            user = CreateNewUser(claims, identityProvider, developmentModeAuth);
        }

        if (user == null || user.Claims == null)
        {
            throw new InvalidOperationException("User or user claims collection was null.");
        }

        var values = user.Claims.ToList();

        var now = DateTime.UtcNow;

        foreach (var item in values)
        {
            if (item.IsValidOnDate(now) == false)
            {
                continue;
            }
            else if (item.ClaimName == ApiConstants.ClaimName_Role)
            {
                AddClaim(claims,
                    ClaimTypes.Role, item.ClaimValue);
            }
            else
            {
                AddClaim(claims,
                    item.ClaimName, item.ClaimValue);
            }
        }

        AddClaim(claims, SecurityConstants.ClaimName_UserId, user.Id.ToString());

        if (string.IsNullOrEmpty(user.FirstName) == true)
        {
            AddClaim(claims, ClaimTypes.GivenName, "??");
        }
        else
        {
            AddClaim(claims, ClaimTypes.GivenName, user.FirstName);
        }
    }

    private User CreateNewUser(List<Claim> claims, string identityProvider, bool developmentModeAuth)
    {
        var info = new UserInformation(new SimpleClaimsAccessor(claims));

        var user = new User
        {
            EmailAddress = info.EmailAddress,
            FirstName = info.FirstName,
            Username = info.Username,
            LastName = info.LastName,
            PhoneNumber = string.Empty,
            Status = ApiConstants.StatusActive,
            Source = identityProvider
        };

        if (developmentModeAuth == true)
        {
            if (info.Username == SecurityConstants.Username_TestAdminUser)
            {
                AddClaim(claims, ClaimTypes.Role, SecurityConstants.RoleName_Admin);
                user.AddRoleClaim(SecurityConstants.RoleName_Admin);
            }
            else
            {
                AddClaim(claims, ClaimTypes.Role, SecurityConstants.RoleName_User);
                user.AddRoleClaim(SecurityConstants.RoleName_User);
            }
        }
        else
        {
            AddClaim(claims, ClaimTypes.Role, SecurityConstants.RoleName_User);
            user.AddRoleClaim(ApiConstants.ClaimValue_Role_User);
        }        

        _userService.Save(user);

        return user;
    }

    private static bool AddClaimsFromHeader(HttpContext context, List<Claim> claims)
    {
        var identityProviderHeader =
            GetHeaderValue(context, SecurityConstants.Claim_X_MsClientPrincipalIdp);

        if (identityProviderHeader != null)
        {
            var identityHeader =
                GetHeaderValue(
                context,
                SecurityConstants.Claim_X_MsClientPrincipalId);

            if (string.IsNullOrEmpty(identityHeader) == false)
            {
                claims.Add(new Claim(
                    SecurityConstants.Claim_X_MsClientPrincipalIdp,
                    identityProviderHeader));

                claims.Add(new Claim(
                    SecurityConstants.Claim_X_MsClientPrincipalId,
                    identityHeader));
            }

            var nameHeader =
                GetHeaderValue(
                context,
                SecurityConstants.Claim_X_MsClientPrincipalName);


            if (string.IsNullOrEmpty(nameHeader) == false)
            {
                claims.Add(new Claim(
                    SecurityConstants.Claim_X_MsClientPrincipalName,
                    nameHeader));

                claims.Add(new Claim(
                    ClaimTypes.Name,
                    nameHeader));
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private static string GetHeaderValue(HttpContext context, string headerName)
    {
        var match = (from temp in context.Request.Headers
                     where temp.Key == headerName
                     select temp.Value).FirstOrDefault();

        return match;
    }
}
