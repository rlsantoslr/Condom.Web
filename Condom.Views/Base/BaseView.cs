using Condom.Domain.Global;
using Condom.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.CondEnum;

namespace Condom.Views.Base
{
    public interface IBaseView
    {
        dynamic GetDynamicDomain();

        CrudEnum GetCrud();


        Tracker GetTracker();

        bool HasError();

    }

    public abstract class BaseView<TView, TDomain> : IBaseView
    {
        protected BaseView()
        {
            Domain = Activator.CreateInstance<TDomain>();
        }

        protected BaseView(TDomain domain)
        {
            Domain = domain;
        }

        public TDomain Domain { get; set; }

        public CrudEnum GetCrud() => GetFunctions().GetCrud();

        public dynamic GetDynamicDomain() => Domain;

        public void SetCrud(CrudEnum crud) => GetFunctions().SetCrud(crud);

        protected IDomainFunctions GetFunctions()
        {
            dynamic d = Domain;
            IDomainFunctions ba = d;
            return d;
        }

        public Tracker GetTracker() => GetFunctions().GetTracker();
        public bool HasError() => GetTracker().HasError();
    }
}
