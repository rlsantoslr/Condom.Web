using Condom.Domain.Models;
using Condom.Domain.Models.Identity;
using Condom.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Views.Models
{
    public class GroupsView : BaseView<GroupsView, Groups>
    {
        public GroupsView()
        {
        }

        public GroupsView(Groups domain) : base(domain)
        {
        }


        public GroupBillings GetBillings()
        {
            if(Domain.Billings == null)
            {
                Domain.Billings = new GroupBillings();
            }

            return Domain.Billings;
        }
    }
}
