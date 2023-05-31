using Condom.Domain.Models;
using Condom.Domain.Models.Identity;
using Condom.Infra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Repositories.Identity
{
    public class UserProfilesStore : Repository<UserProfiles>
    {
        public UserProfilesStore(CondomContext context) : base(context)
        {
        }
    }
}
