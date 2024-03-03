using Benday.Presidents.Api.Interfaces;
using System;
using System.Linq;

namespace Benday.Presidents.UnitTests.Features
{
    public class MockUsernameProvider : IUsernameProvider
    {
        public string ReturnThisUsername { get; set; }

        public string GetUsername()
        {
            return ReturnThisUsername;
        }
    }
}
