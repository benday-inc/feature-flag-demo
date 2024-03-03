using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Benday.WebSecurity
{
    public class AzureEasyAuthClient
    {
        private HttpClient _client;

        public AzureEasyAuthClient(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), $"{nameof(request)} is null.");
            }

            TryInitializeHttpClientUsingSessionCookie(request);
        }

        public bool IsReadyForAuthenticatedCall
        {
            get;
            private set;
        }

        private void TryInitializeHttpClientUsingSessionCookie(HttpRequest request)
        {
            var requestCookies = request.Cookies;

            if (requestCookies.ContainsKey(SecurityConstants.Cookie_AppServiceAuthSession) == false)
            {
                IsReadyForAuthenticatedCall = false;
            }
            else
            {
                var handler = new HttpClientHandler();

                var client = new HttpClient(handler);

                var baseUrl = $"{request.Scheme}://{request.Host}";

                client.BaseAddress = new Uri(baseUrl);

                var container = new CookieContainer();

                handler.CookieContainer = container;

                var authCookie =
                    requestCookies[SecurityConstants.Cookie_AppServiceAuthSession];

                container.Add(
                    new Uri(baseUrl),
                    new Cookie(
                    SecurityConstants.Cookie_AppServiceAuthSession,
                    authCookie));

                IsReadyForAuthenticatedCall = true;

                _client = client;
            }
        }

        public string GetUserInformationJson()
        {
            if (IsReadyForAuthenticatedCall == false)
            {
                return null;
            }
            else
            {
                var resultAsString = _client.GetStringAsync("/.auth/me").Result;

                return resultAsString;
            }
        }
    }
}
