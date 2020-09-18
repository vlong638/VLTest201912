using FrameworkTest.Common.ValuesSolution;
using System;
using System.Linq;

namespace FrameworkTest.Business.ExcelGenerator
{
    public class PlaceHolder
    {
        public PlaceHolder(string text)
        {
            if (text.StartsWith("@LoopAuto"))
            {
                var ss = text.TrimStart("@LoopAuto");
                var values = ss.Split('_');
                if (values.Count() != 3)
                    return;
                LoopAuto = true;
                Source = values[1];
                Field = values[2];
            }
            else if (text.StartsWith("@Loop"))
            {
                var ss = text.TrimStart("@Loop");
                var values = ss.Split('_');
                if (values.Count() != 3)
                    return;
                Loop = values[0].ToInt().Value;
                Source = values[1];
                Field = values[2];
            }
            else
            {
                var ss = text.TrimStart("@");
                var values = ss.Split('_');
                if (values.Count() != 2)
                    return;
                Source = values[0];
                Field = values[1];
            }
        }

        public string Source { set; get; } = "";
        public string Field { set; get; } = "";
        public int Loop { set; get; }
        public bool LoopAuto { set; get; }
    }
}
