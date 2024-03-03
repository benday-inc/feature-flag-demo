using System;
using System.Collections.Generic;
using System.Linq;
using Benday.Presidents.Api.Models;

namespace Benday.Presidents.WebUI.Models
{
    public class PresidentToSearchResultRowAdapter
    {
        public void Adapt(President fromValue, SearchResultRow toValue)
        {
            if (fromValue == null)
                throw new ArgumentNullException("fromValue", "fromValue is null.");
            if (toValue == null)
                throw new ArgumentNullException("toValue", "toValue is null.");

            toValue.Id = fromValue.Id;
            toValue.LastName = fromValue.LastName;
            toValue.FirstName = fromValue.FirstName;
        }
    }
}
