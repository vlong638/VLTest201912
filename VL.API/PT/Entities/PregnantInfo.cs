using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.API.PT.Entities
{
    [Table(nameof(PregnantInfo))]//注意 dapper会在表名后默认加s 需指定表名
    public class PregnantInfo
    {
        public int Id { set; get; }
        public string PersonName { set; get; }
        public string Photo { set; get; }
    }
}
