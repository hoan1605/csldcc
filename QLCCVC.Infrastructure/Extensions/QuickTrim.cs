using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCCVC.Infrastructure.Extensions
{
    public static class QuickTrim
    {
        public static string NullSafeTrim(this string item)
        {
            if (item == null)
                return null;

            return item.Trim();
        }

        public static void NullSafeTrimStrings_Initial<T>(this T item)
        {
            Type itemType = typeof(T);

            var props = PropertyCache.Instance.GetProperties(itemType);

            foreach (var prop in props)
            {
                string currentValue = (string)prop.GetValue(item, null);
                prop.SetValue(item, currentValue.NullSafeTrim(), null);
            }
        }
    }
}
