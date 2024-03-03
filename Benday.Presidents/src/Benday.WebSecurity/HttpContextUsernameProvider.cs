using Benday.CodeGenerator.Api;

namespace Benday.WebSecurity;


public class HttpContextUsernameProvider : IUsernameProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextUsernameProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor),
            $"{nameof(contextAccessor)} is null.");
    }

    public string Username => GetUsername();

    public string GetUsername()
    {
        var context = _contextAccessor.HttpContext;

        if (context != null &&
            context.User != null &&
            context.User.Identity != null &&
            string.IsNullOrWhiteSpace(context.User.Identity.Name) == false)
        {
            return
                ApiUtilities.SafeToString(context.User.Identity.Name,
                "(unknown username)");
        }
        else
        {
            return "(unknown username)";
        }
    }
}
