using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.CondEnum;

namespace Condom.Domain.Global
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidatorAttribute : Attribute
    {
        public CrudEnum[] Applicability { get; private set; }
        public ValidatorAttribute(CrudEnum[] applicability)
        {
            Applicability = applicability;
        }
    }
}
