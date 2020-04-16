using System;
using System.Collections.Generic;
using System.Text;

namespace VL.Consoling.Entities
{
    public class SelectDifferent
    {
        public Reflect SourceReflect { set; get; }
        public Select SourceSelect { set; get; }
        public Option SourceOption { set; get; }

        public Reflect TargetReflect { set; get; }
        public Select TargetSelect{ set; get; }
        public Option TargetOption { set; get; }

        public int DifferentType { set; get; }
    }
}
