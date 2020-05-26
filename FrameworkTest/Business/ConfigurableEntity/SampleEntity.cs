using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.ConfigurableEntity
{
    public class SampleEntity
    {
        public long Id { get { return KeyValues[nameof(Id)].ToLong().Value; } }

        public Dictionary<string, object> KeyValues { set; get; } = new Dictionary<string, object>();
    }

    public static class ObjectEx
    {
        public static int? ToInt(this object item)
        {
            if (item == null)
                return null;
            return int.Parse(item.ToString());
        }

        public static long? ToLong(this object item)
        {
            if (item==null)
                return null;
            return long.Parse(item.ToString());
        }

        public static DateTime? ToDateTime(this object item)
        {
            if (item == null)
                return null;
            return DateTime.Parse(item.ToString());
        }
    }
}
