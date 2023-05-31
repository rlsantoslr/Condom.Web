using Condom.Domain.Global;
using Condom.Domain.Models;
using Condom.Infra.App.Base;
using Condom.Infra.Repositories.Identity;
using Condom.Infra.Sender;
using Condom.Infra.Validations;
using Condom.Infra.Validations.Base;
using Condom.Views.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Condom.Infra.App
{
    public class IdentityApp : BaseApp<UsersView, Users>
    {
        readonly IIdentityUserStore<Users> _IdentityUserStore;
        readonly UserValidator _UserValidator;
        readonly UserManager<Users> _UserManager;
        readonly SignInManager<Users> _SignInManager;
        readonly UserProfilesStore _UserProfilesStore;
        readonly MailSender MailSender;

        public IdentityApp(IIdentityUserStore<Users> identityUserStore, UserValidator userValidator, UserManager<Users> usermanager, SignInManager<Users> signInManager, UserProfilesStore userProfilesStore, MailSender mailSender) : base(userValidator)
        {
            _IdentityUserStore = identityUserStore ?? throw new ArgumentNullException(nameof(identityUserStore));
            _UserValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
            _UserManager = usermanager ?? throw new ArgumentNullException(nameof(usermanager));
            _SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _UserProfilesStore = userProfilesStore;
            MailSender = mailSender;
        }

        protected override async Task<UsersView> OnAfterValidate(UsersView view)
        {
            switch (view.GetCrud())
            {
                case Domain.Global.CondEnum.CrudEnum.Create:

                    var rCreate = await _UserManager.CreateAsync(view.Domain, view.Password);
                    view.GetTracker().AddLog(rCreate);
                    if (rCreate.Succeeded)
                    {
                        await _UserProfilesStore.CreateAsync(view.Domain.UserProfile);
                        view.GetTracker().Merge(await _UserProfilesStore.SaveChangeAsync());

                        await SendConfirmationEmail(view);
                    }
                    break;

                case Domain.Global.CondEnum.CrudEnum.Read:
                    if (!view.Domain.EmailConfirmed)
                    {
                        view.GetTracker().AddLog(MessageTypeEnum.Error, "O e-mail ainda não foi confirmado!");
                        await SendConfirmationEmail(view);
                        return view;
                    }

                    var result = await _SignInManager.PasswordSignInAsync(view.Domain.Email, view.Password, true, true);
                    view.GetTracker().AddLog(result);
                    break;
            }

            return view;
        }

        protected async Task SendConfirmationEmail(UsersView view)
        {
            var user = await _IdentityUserStore.FindByEmail(view.Domain.Email);
            var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
            token = HttpUtility.UrlEncode(token);
            await MailSender.SendConfirmation(view.Domain.Email, token, view.Name);
        }

        public async Task<Tracker> ResetPassword(UsersView model)
        {
            model.GetTracker().Merge(await _UserValidator.ExternalValidateProperty<UsersView>(model, nameof(model.Password), CondEnum.CrudEnum.Create, model));
            if (model.HasError()) return model.GetTracker();

            model.GetTracker().Merge(await _UserValidator.ExternalValidateProperty<UsersView>(model, nameof(model.ConfirmPassword), CondEnum.CrudEnum.Create, model));
            if (model.HasError()) return model.GetTracker();

            var user = await _IdentityUserStore.FindByEmail(model.Email);
            if (user == null)
            {
                model.GetTracker().AddLog(Domain.Global.MessageTypeEnum.Error, "Não foi possivel realizar essa atividade, tente novamente mais tarde");
                return model.GetTracker();
            }

            model.GetTracker().AddLog(await _UserManager.ResetPasswordAsync(user, model.Token, model.Password));

            return model.GetTracker();
        }

        public async Task<Tracker> ForgotPassword(string email)
        {
            Tracker tracker = new Tracker();
            var user = await _IdentityUserStore.FindByEmail(email);
            if (user == null)
            {
                tracker.AddLog(Domain.Global.MessageTypeEnum.Error, "Não foi possivel concluir a sua solicitação");
                return tracker;
            }

            var token = await _UserManager.GeneratePasswordResetTokenAsync(user);
            token = HttpUtility.UrlEncode(token);
            await MailSender.SendForgetPassword(email, token);

            tracker.AddLog(MessageTypeEnum.Info, "Te enviamos um e-mail para que você possa redefinir a sua senha, lembre de verificar a caixa de spam");

            return tracker;
        }

        public async Task<UsersView> AccountConfirm(UsersView view)
        {
            var user = await _IdentityUserStore.FindByEmail(view.Domain.Email);
            if (user == null)
            {
                view.GetTracker().AddLog(Domain.Global.MessageTypeEnum.Error, "Não foi possivel confirmar a sua conta, tente novamente mais tarde");
                return view;
            }

            user.GetTracker().AddLog(await _UserManager.ConfirmEmailAsync(user, view.Token));

            return view;
        }
    }
}
