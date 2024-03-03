using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Benday.WebSecurity
{
    public class SecurityUtility
    {
        private readonly ClaimsIdentity _identity;
        private readonly IPrincipal _principal;

        public SecurityUtility(IIdentity identity, IPrincipal principal)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity), "identity is null.");

            _identity = identity as ClaimsIdentity;
            _principal = principal ?? throw new ArgumentNullException(nameof(principal), "principal is null.");
        }

        public bool IsInRole(string role)
        {
            if (_principal == null)
            {
                return false;
            }
            else
            {
                return _principal.IsInRole(role);
            }
        }

        public bool IsAuthorized(string permissionName, int id,
            bool checkForAdminRole = true)
        {
            return IsAuthorized(permissionName, id, checkForAdminRole);
        }

        public bool IsAuthorized(string permissionName, string id, bool checkForAdminRole)
        {
            if (_identity == null)
            {
                return false;
            }
            else if (checkForAdminRole == true)
            {
                if (IsAuthorized(SecurityConstants.RoleName_Admin) == true)
                {
                    return true;
                }
                else
                {
                    return _identity.HasClaim(permissionName, id);
                }
            }
            else
            {
                return _identity.HasClaim(permissionName, id);
            }
        }

        public bool IsAuthorized(string roleName)
        {
            if (_identity == null)
            {
                return false;
            }
            else
            {
                return _identity.HasClaim(ClaimTypes.Role, roleName);
            }
        }

        public bool HasClaim(string claimType, string claimValue)
        {
            if (_identity == null)
            {
                return false;
            }
            else
            {
                return _identity.HasClaim(claimType, claimValue);
            }
        }

        public bool HasClaim(string claimType)
        {
            if (_identity == null)
            {
                return false;
            }
            else
            {
                return _identity.HasClaim(c => c.Type == claimType);
            }
        }
    }
}
