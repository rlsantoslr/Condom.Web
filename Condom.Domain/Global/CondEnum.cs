using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Domain.Global
{
    public static class CondEnum
    {
        public enum CrudEnum
        {
            None = 0,
            Create = 1,
            Read = 2,
            Update = 3,
            Delete = 4
        }
    }
}
