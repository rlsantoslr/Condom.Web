using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Domain.Models
{
    public interface IBaseModel<T> where T : class
    {
        abstract void Overwrite(T domain);
    }

    public abstract class BaseModel<T> : Entity, IBaseModel<T> where T : class
    {
        public BaseModel()
        {
        }

        public BaseModel(T domain)
        {
            if (domain == null) return;
            Overwrite(domain);
        }
        public abstract void Overwrite(T domain);
    }
}
