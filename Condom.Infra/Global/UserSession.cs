using Condom.Domain.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Global
{
    public class UserSession
    {
        public string Name => GetUser().Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? "";

        public Guid UserId => Guid.Parse(GetUser().Claims.FirstOrDefault(x => x.Type == "userid")?.Value ?? Guid.Empty.ToString());

        private readonly AuthenticationStateProvider _State;

        public UserSession(AuthenticationStateProvider provider, SignInManager<Users> _SignInManager)
        {
            _State = provider;
        }

        public ClaimsPrincipal GetUser()
        {
            var task = _State.GetAuthenticationStateAsync();
            task.Wait();

            return task.Result.User;
        }

        public string GetName()
        {
            return Name;
        }

        public Guid GetUserId()
        {
            return UserId;
        }
    }
}
