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
            int i;
            if (int.TryParse(item.ToString(), out i))
                return i;
            return null;
        }

        public static long? ToLong(this object item)
        {
            if (item==null)
                return null;
            long l;
            if (long.TryParse(item.ToString(), out l))
                return l;
            return null;
        }

        public static DateTime? ToDateTime(this object item)
        {
            if (item == null)
                return null;
            DateTime dt;
            if (DateTime.TryParse(item.ToString(), out dt))
                return dt;
            return null;
        }
    }
}
