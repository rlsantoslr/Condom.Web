using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Helpers
{
    public static class Tools
    {
        public static List<KeyValuePair<string, string>> GetAllEntities<T>([Optional] T t, Type ty = null, bool justInterface = false)
        {
            var rt = new List<KeyValuePair<string, string>>();

            Type from = ty;
            if (from == null) from = typeof(T);

            foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => from.IsAssignableFrom(x) && x.IsInterface == justInterface && x.IsAbstract == justInterface))
            {
                var pair = new KeyValuePair<string, string>(type?.FullName, type?.AssemblyQualifiedName);
                rt.Add(pair);
            }

            return rt;
        }
    }
}
