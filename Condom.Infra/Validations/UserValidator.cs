using Condom.Domain.Global;
using Condom.Domain.Models;
using Condom.Infra.Global;
using Condom.Infra.Validations.Base;
using Condom.Views.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Validations
{
    public class UserValidator : BaseValidator<UsersView, Users>
    {
        public UserValidator(UserSession session) : base(session)
        {
        }

        protected override async Task<Tracker> ValidateProperty(Tracker tracker, PropertyInfo property, dynamic value)
        {
            switch (property.Name)
            {
                case nameof(Users.Email):
                    break;
            }

            return tracker;
        }
    }
}
