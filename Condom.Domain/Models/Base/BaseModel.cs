using Condom.Domain.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.CondEnum;

namespace Condom.Domain.Models
{
    public interface IDomainFunctions
    {
        CrudEnum GetCrud();

        void SetCrud(CrudEnum crud);

        Tracker GetTracker();
    }

    public interface IBaseModel<T> : IDomainFunctions where T : class
    {
        abstract void Overwrite(T domain);
    }

    public abstract class BaseModel<T> : Entity, IBaseModel<T> where T : class
    {
        protected CrudEnum Crud { get; set; }

        protected Tracker _Tracker { get; set; } = new Tracker();

        public BaseModel()
        {
        }

        public BaseModel(T domain)
        {
            if (domain == null) return;
            Overwrite(domain);
        }

        public CrudEnum GetCrud() => Crud;

        public void SetCrud(CrudEnum crud) => Crud = crud;

        public abstract void Overwrite(T domain);

        public Tracker GetTracker() => _Tracker;
    }
}
