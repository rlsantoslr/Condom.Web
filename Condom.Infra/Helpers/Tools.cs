using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public static DropDown<T> ConvertEnumToDropDown<T>(T[] ignore)
        {
            var dropDown = new DropDown<T>();
            var values = Enum.GetValues(typeof(T));
            foreach(var v in values)
            {
                if(ignore != null)
                {
                    if(ignore.ToList().Exists(x => x.Equals(v)))
                    {
                        continue;
                    }
                }

                var display = GetEnumDisplay(v);

                dynamic dv = v;

                dropDown.Data.Add(dv, display);
            }

            return dropDown;

        }

        public static string GetEnumDisplay<T>(T value)
        {
            var display = value.ToString();

            var dAttr = value.GetType()?.GetMember(value.ToString())?.First()?.GetCustomAttribute<DisplayAttribute>();
            if (dAttr != null)
            {
                display = dAttr.GetName();
            }

            return display;
        }
    }
}
