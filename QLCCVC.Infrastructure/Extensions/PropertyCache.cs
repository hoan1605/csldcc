using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QLCCVC.Infrastructure.Extensions
{
    public class PropertyCache
    {
        private static readonly PropertyCache instance = new PropertyCache();
        private static readonly Dictionary<Type, PropertyInfo[]> cachedItems = new Dictionary<Type, PropertyInfo[]>();

        private PropertyCache() { }

        public static PropertyCache Instance
        {
            get { return instance; }
        }

        public PropertyInfo[] GetProperties(Type t)
        {
            if (!cachedItems.ContainsKey(t))
                cachedItems[t] = t.GetProperties()
                        .Where(x => x.CanRead && x.CanWrite && x.PropertyType == typeof(string))
                        .ToArray();

            return cachedItems[t];
        }
    }
}
