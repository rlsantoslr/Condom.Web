using Condom.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Claims
{
    public class CustomClaimFactory : UserClaimsPrincipalFactory<Users>
    {
        public CustomClaimFactory(
            UserManager<Users> userManager,
            IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, optionsAccessor)
        {
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Users user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            //identity.AddClaim(new Claim("name", user.Name));

            identity.AddClaim(new Claim("userid", user.Id.ToString()));

            return identity;
        }
    }
}
