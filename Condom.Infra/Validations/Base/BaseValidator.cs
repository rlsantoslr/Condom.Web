using Condom.Domain.Global;
using Condom.Domain.Models;
using Condom.Infra.Global;
using Condom.Infra.Repositories;
using Condom.Views.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.CondEnum;

namespace Condom.Infra.Validations.Base
{
    public interface IBaseValidator<TView, TDomain>
    {
        Task<TView> OnAction(TView view);

        BaseValidator<TView, TDomain> GetBase();
    }

    public abstract class BaseValidator<TView, TDomain> : IBaseValidator<TView, TDomain>
    {
        readonly UserSession Session;

        public BaseValidator(UserSession session)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public BaseValidator<TView, TDomain> GetBase()
        {
            return this;
        }

        public async Task<TView> OnAction(TView view)
        {

            dynamic dview = view;
            IBaseView baseView = dview;

            baseView.GetTracker().Reset();

            dynamic ddomain = baseView.GetDynamicDomain();
            
            CrudEnum crud = baseView.GetCrud();
            var tracker = baseView.GetTracker();

            //Validar DOMAIN
            baseView.GetTracker().Merge(await CheckProperties<TDomain>(ddomain, crud, new Tracker()));
            if (baseView.HasError()) return view;

            //Validar VIEW
            baseView.GetTracker().Merge(await CheckProperties(view, crud, new Tracker()));
            if (baseView.HasError()) return view;

            return view;
        }
        protected async Task<Tracker> CheckProperties<T>(T context, CrudEnum crud, Tracker tracker)
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var displayName = property.Name;
                var attr = property.GetCustomAttribute<ValidatorAttribute>();
                if (attr == null) continue;

                if (!attr.Applicability.Contains(crud)) continue;

                var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
                if (displayAttr != null)
                {
                    displayName = displayAttr.Name;
                }

                var val = property.GetValue(context);

                if (!CheckAttributes(property, val, displayName, tracker))
                {
                    return tracker;
                }

                tracker.Merge(await ValidateProperty(new Tracker(), property, val));

                if (tracker.HasError()) return tracker;
            }

            return tracker;
        }
        protected bool CheckAttributes(PropertyInfo property, object? value, string field, Tracker tracker)
        {
            var max = property.GetCustomAttribute<MaxLengthAttribute>();
            if (max != null)
            {
                if (!max.IsValid(value))
                {
                    tracker.AddLog(MessageTypeEnum.Error, $"O tamanho do campo {field} excede o limite máximo de {max.Length.ToString()}");
                    return false;
                }
            }

            var req = property.GetCustomAttribute<RequiredAttribute>();
            if (req != null)
            {
                if (!req.IsValid(value))
                {
                    tracker.AddLog(MessageTypeEnum.Error, $"O campo {field} não pode ser nulo ou branco");
                    return false;
                }
            }

            var range = property.GetCustomAttribute<RangeAttribute>();
            if (range != null)
            {
                if (!range.IsValid(value))
                {
                    tracker.AddLog(MessageTypeEnum.Error, $"O campo {field} deve conter um valor entre {range.Minimum} e {range.Maximum}");
                    return false;
                }
            }

            var phone = property.GetCustomAttribute<PhoneAttribute>();
            if (phone != null)
            {
                if (!phone.IsValid(value))
                {
                    tracker.AddLog(MessageTypeEnum.Error, $"O campo {field} número do telefone não é válido");
                    return false;
                }
            }

            var regex = property.GetCustomAttribute<RegularExpressionAttribute>();
            if (regex != null)
            {
                if (!regex.IsValid(value))
                {
                    tracker.AddLog(MessageTypeEnum.Error, $"O campo {field} não atende o padrão ({regex.Pattern})");
                    return false;
                }
            }

            var url = property.GetCustomAttribute<UrlAttribute>();
            if (url != null)
            {
                if (!url.IsValid(value))
                {
                    tracker.AddLog(MessageTypeEnum.Error, $"O campo {field} não possui uma url válida");
                    return false;
                }
            }

            var email = property.GetCustomAttribute<EmailAddressAttribute>();
            if (email != null)
            {
                if (!email.IsValid(value))
                {
                    tracker.AddLog(MessageTypeEnum.Error, $"O E-mail não é válido");
                    return false;
                }
            }

            var pass = property.GetCustomAttribute<PasswordAttribute>();
            if (pass != null)
            {
                if (!pass.IsValid(value))
                {
                    tracker.AddLog(MessageTypeEnum.Error, $"O campo {field} não atende os critérios de segurança (Deve possuir letra mínuscula, letra maiúscula, número e caractér especial)");
                    return false;
                }
            }

            return true;
        }
        protected abstract Task<Tracker> ValidateProperty(Tracker tracker, PropertyInfo property, dynamic? value);
    }
}
