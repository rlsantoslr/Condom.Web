using Condom.Domain.Global;
using Condom.Domain.Models;
using Condom.Infra.Global;
using Condom.Infra.Repositories.Identity;
using Condom.Infra.Sender;
using Condom.Infra.Validations.Base;
using Condom.Views.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        readonly IIdentityUserStore<Users> _UserStore;
        readonly UserManager<Users> _UserManager;
        readonly MailSender _Sender;
        public UserValidator(UserSession session, IIdentityUserStore<Users> userStore, UserManager<Users> userManager, MailSender sender) : base(session)
        {
            _UserStore = userStore;
            _UserManager = userManager;
            _Sender = sender;
        }

        public override async Task<UsersView> OnAfterPropertiesValidation(UsersView view, CondEnum.CrudEnum crud)
        {
            switch (crud)
            {
                case CondEnum.CrudEnum.Create:
                    view.Domain.Id = Guid.NewGuid();
                    view.UserId = view.Domain.Id;
                    view.Domain.UserName = view.Domain.Email;
                    view.Domain.NormalizedEmail = view.Domain.Email.ToUpperInvariant().Trim();
                    view.Domain.NormalizedUserName = view.Domain.Email.ToUpperInvariant().Trim();
                    if (await _UserStore.CountAsync(x => x.NormalizedEmail == view.Domain.NormalizedEmail) > 0)
                    {
                        view.GetTracker().AddLog(MessageTypeEnum.Error, "Já existe um usuário com esse e-mail");
                        return view;
                    }
                    break;
                case CondEnum.CrudEnum.Update:
                    break;
                case CondEnum.CrudEnum.Delete:
                    break;
                case CondEnum.CrudEnum.Read:
                    var user = await _UserStore.FindByEmail(view.Domain.Email);
                    if(user == null)
                    {
                        view.GetTracker().AddLog(MessageTypeEnum.Error, "Usuário não encontrado!");
                    }
                    else
                    {
                        view.Domain.Overwrite(user);
                    }
                    break;
            }

            return view;
        }

        protected override async Task<bool> ValidateSession(CondEnum.CrudEnum crud, UsersView view)
        {
            switch (crud)
            {
                case CondEnum.CrudEnum.Create:
                    return false;
                case CondEnum.CrudEnum.Read:
                    return false;
            }
            return true;
        }

        protected override async Task<Tracker> ValidateProperty(Tracker tracker, PropertyInfo property, dynamic value, UsersView view)
        {
            switch (property.Name)
            {
                case nameof(UsersView.ConfirmPassword):
                    if (!view.ConfirmPassword.Equals(view.Password))
                    {
                        tracker.AddLog(MessageTypeEnum.Error, "A senha está diferente da confirmação");
                    }
                    break;
            }

            return tracker;
        }
    }
}
