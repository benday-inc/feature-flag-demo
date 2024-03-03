using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Benday.WebSecurity
{
    public class HttpContextClaimsAccessor : IClaimsAccessor
    {
        private readonly IHttpContextAccessor _accessor;

        public HttpContextClaimsAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        private HttpContext _context;
        private HttpContext Context
        {
            get
            {
                if (_context == null)
                {
                    if (_accessor != null)
                    {
                        _context = _accessor.HttpContext;
                    }
                }

                return _context;
            }
        }

        private IEnumerable<Claim> _claims;
        public IEnumerable<Claim> Claims
        {
            get
            {
                if (_claims == null)
                {
                    if (Context == null || Context.User == null || Context.User.Claims == null)
                    {
                        _claims = new List<Claim>();
                    }
                    else
                    {
                        _claims = Context.User.Claims.ToList();
                    }
                }

                return _claims;
            }
        }
    }
}
