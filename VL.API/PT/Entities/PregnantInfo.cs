using Dapper.Contrib.Extensions;
using VL.API.Common.Entities;

namespace VL.API.PT.Entities
{
    [Table(nameof(PregnantInfo))]//注意 dapper会在表名后默认加s 需指定表名
    public class PregnantInfo: IDataValidation
    {
        public int Id { set; get; }
        public string PersonName { set; get; }
        public string Photo { set; get; }

        public ValidateResult Validate()
        {
            return ValidateResult.Empty;
        }
    }
}
