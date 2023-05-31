using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Helpers
{
    public class DropDown<T>
    {
        public Dictionary<T, string> Data { get; protected set; } = new Dictionary<T, string>();

        public DropDown()
        {
        }
    }
}
