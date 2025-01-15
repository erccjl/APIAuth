using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Aplication.Policies
{
    public class AuthorizationPolicies
    {
        public static void ConfigurePolicies(AuthorizationOptions options)
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin"));

            options.AddPolicy("UserAndAdmin", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));

            options.AddPolicy("ActiveUser", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "isActive" && c.Value == "true")));
        }
    }
}
