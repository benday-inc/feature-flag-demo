using System.Collections.Generic;
using System.Security.Claims;

namespace Benday.WebSecurity
{
    public interface IClaimsAccessor
    {
        IEnumerable<Claim> Claims { get; }
    }
}
