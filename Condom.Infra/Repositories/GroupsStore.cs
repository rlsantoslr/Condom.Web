using Condom.Domain.Models;
using Condom.Infra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Repositories
{
    public class GroupsStore : Repository<Groups>
    {
        public GroupsStore(CondomContext context) : base(context)
        {
        }
    }
}
