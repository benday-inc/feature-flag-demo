using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.Presidents.Api.Interfaces
{
    public interface IFeatureManager
    {
        bool Search { get; }

        bool SearchByBirthDeathState { get; }

        bool FeatureUsageLogging { get; }

        bool CustomerSatisfaction { get; }
    }
}
