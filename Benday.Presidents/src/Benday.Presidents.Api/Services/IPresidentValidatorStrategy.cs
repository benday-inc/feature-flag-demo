using System;
using System.Collections.Generic;
using System.Linq;
using Benday.Presidents.Api.Models;

namespace Benday.Presidents.Api.Services
{
    public interface IPresidentValidatorStrategy
    {
        bool IsValid(President validateThis);
    }
}
