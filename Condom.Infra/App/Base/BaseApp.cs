using Condom.Infra.Validations.Base;
using Condom.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.App.Base
{
    public class BaseApp<TView, TDomain>
    {
        readonly BaseValidator<TView, TDomain> Validator;

        public BaseApp(BaseValidator<TView, TDomain> validator)
        {
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<TView> OnAction(TView view)
        {

            view = await Validator.OnAction(view);

            dynamic d = view;

            IBaseView ib = d;

            if (ib.GetTracker().HasError()) return view;


            switch (ib.GetCrud())
            {
                case Domain.Global.CondEnum.CrudEnum.Create:
                    break;
                case Domain.Global.CondEnum.CrudEnum.Update:
                    break;
                case Domain.Global.CondEnum.CrudEnum.Read:
                    break;
                case Domain.Global.CondEnum.CrudEnum.Delete:
                    break;
            }

            return view;
        }
    }
}
