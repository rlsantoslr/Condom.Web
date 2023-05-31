using Condom.Domain.Global;
using Condom.Domain.Models;
using Condom.Infra.Global;
using Condom.Infra.Validations.Base;
using Condom.Views.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Validations
{
    public class GroupValidator : BaseValidator<GroupsView, Groups>
    {
        public GroupValidator(UserSession session) : base(session)
        {
        }

        public override async Task<GroupsView> OnAfterPropertiesValidation(GroupsView view, CondEnum.CrudEnum crud)
        {
            switch (crud)
            {
                case CondEnum.CrudEnum.Create:
                    view.Domain.Id = Guid.NewGuid();
                    view.Domain.CreatedAt = DateTime.Now;
                    view.Domain.OwnerId = Session.UserId;
                    break;
                case CondEnum.CrudEnum.Update:
                    break;
                case CondEnum.CrudEnum.Delete:
                    break;
                case CondEnum.CrudEnum.Read:

                    break;
            }

            return view;
        }

        protected override async Task<Tracker> ValidateProperty(Tracker tracker, PropertyInfo property, dynamic value, GroupsView view)
        {
            return tracker;
        }
    }
}
