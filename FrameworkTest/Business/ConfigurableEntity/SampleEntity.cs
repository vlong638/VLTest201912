using FrameworkTest.Common.ValuesSolution;
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
    }
}
