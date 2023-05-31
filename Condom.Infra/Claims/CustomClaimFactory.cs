using Condom.Domain.Models;
using Condom.Infra.Repositories.Identity;
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
        readonly IIdentityUserStore<Users> _UserStore;
        public CustomClaimFactory(
            IIdentityUserStore<Users> identityUserStore,
            UserManager<Users> userManager,
            IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, optionsAccessor)
        {
            _UserStore = identityUserStore;
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Users user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var profile = await _UserStore.GetProfileByUserId(user.Id);
            if(profile != null)
            {
                identity.AddClaim(new Claim("name", profile.Name));
            }


            identity.AddClaim(new Claim("userid", user.Id.ToString()));

            return identity;
        }
    }
}
