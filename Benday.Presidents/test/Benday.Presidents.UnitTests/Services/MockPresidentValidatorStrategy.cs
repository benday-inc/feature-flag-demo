using System;
using System.Linq;
using Benday.Presidents.Api.Services;
using System.Collections.Generic;
using Benday.Presidents.Api.Models;

namespace Benday.Presidents.UnitTests.Services
{
    public class MockPresidentValidatorStrategy : IPresidentValidatorStrategy
    {
        public MockPresidentValidatorStrategy()
        {
            IsValidReturnValue = true;
        }

        public bool IsValidReturnValue { get; set; }

        public bool IsValid(President validateThis)
        {
            return IsValidReturnValue;
        }
    }
}
