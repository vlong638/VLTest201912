using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VL.Research.Common;

namespace VL.Research.Models
{
    public class GetListConfigModel
    {
        public long CustomConfigId { set; get; }
        public ViewConfig ViewConfig { set; get; }
    }
}
