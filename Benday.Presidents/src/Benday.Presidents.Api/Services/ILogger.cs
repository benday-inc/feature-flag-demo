using Benday.Presidents.Common;
using Benday.Presidents.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benday.Presidents.Api.Services
{
    public interface ILogger
    {
        void LogFeatureUsage(string featureName);
        void LogCustomerSatisfaction(string feedback);
    }
}
