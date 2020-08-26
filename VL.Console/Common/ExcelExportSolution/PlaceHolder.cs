using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    public class PlaceHolder
    {
        public PlaceHolder(string text)
        {
            if (text.StartsWith("@Loop"))
            {
                var ss = text.TrimStart("@Loop");
                var values = ss.Split('_');
                if (values.Count() != 3)
                    return;
                Loop = values[0].ToInt().Value;
                Source = values[1];
                Field = values[2];
            }
            else if (text.StartsWith("@Sum"))
            {
                var ss = text.TrimStart("@Sum_");
                var values = ss.Split('_');
                if (values.Count() != 3)
                    return;
                Func = values[0];
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
        public string Func { set; get; }
    }
}
