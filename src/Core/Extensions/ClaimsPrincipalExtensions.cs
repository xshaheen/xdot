using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using X.Core.Extensions;

// ReSharper disable once CheckNamespace
namespace System.Security.Claims {
    [PublicAPI]
    public static class ClaimsPrincipalExtensions {
        /// <summary>
        /// Get the <paramref name="claimType"/> claim.
        /// </summary>
        public static Claim? Claim(this ClaimsPrincipal? principal, string claimType) {
            return principal?.FindFirst(claimType);
        }

        /// <summary>
        /// Get all claims of type <paramref name="claimType"/>
        /// </summary>
        public static IList<Claim>? Claims(
            this ClaimsPrincipal? claimsPrincipal,
            string claimType
        ) {
            return claimsPrincipal?.FindAll(claimType).ToList();
        }

        /// <summary>
        /// Get user roles collection.
        /// </summary>
        public static IList<string>? Roles(
            this ClaimsPrincipal? principal,
            string claimType = ClaimTypes.Role
        ) {
            return principal?.Claims.Where(c => c.Type == claimType).Select(c => c.Value).ToList();
        }

        /// <summary>
        /// Get user roles as Enum collection of type <typeparamref name="T"/>.
        /// </summary>
        public static List<T>? Roles<T>(
            this ClaimsPrincipal? principal,
            string claimType = ClaimTypes.Role
        ) where T : struct {
            return principal?.Claims.Where(c => c.Type == claimType)
                .Select(c => c.Value.ToEnum<T>()).ToList();
        }
    }
}
