using Condom.Domain.Models;
using Condom.Infra.App.Base;
using Condom.Infra.Repositories.Identity;
using Condom.Infra.Validations;
using Condom.Infra.Validations.Base;
using Condom.Views.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.App
{
    public class IdentityApp : BaseApp<UsersView, Users>
    {
        readonly IIdentityUserStore<Users> _IdentityUserStore;
        readonly UserValidator _UserValidator;

        public IdentityApp(IIdentityUserStore<Users> identityUserStore, UserValidator userValidator) : base(userValidator)
        {
            _IdentityUserStore = identityUserStore ?? throw new ArgumentNullException(nameof(identityUserStore));
            _UserValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
        }
    }
}
