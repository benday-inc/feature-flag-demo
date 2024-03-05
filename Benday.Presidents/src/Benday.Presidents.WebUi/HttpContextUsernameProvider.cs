using Benday.Presidents.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Benday.Presidents.WebUi;


public class HttpContextUsernameProvider : IUsernameProvider
{
    private readonly IHttpContextAccessor _ContextAccessor;

    public HttpContextUsernameProvider(IHttpContextAccessor contextAccessor)
    {
        _ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor),
            $"{nameof(contextAccessor)} is null.");
    }

    public string Username => GetUsername();

    public string GetUsername()
    {
        var context = _ContextAccessor.HttpContext;

        if (context != null &&
            context.User != null &&
            context.User.Identity != null &&
            string.IsNullOrWhiteSpace(context.User.Identity.Name) == false)
        {
            return
                Benday.Common.StringExtensionMethods.SafeToString(context.User.Identity.Name,
                "(unknown username)");
        }
        else
        {
            return "(unknown username)";
        }
    }
}