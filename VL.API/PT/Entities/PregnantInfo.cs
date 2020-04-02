using Dapper.Contrib.Extensions;
using VL.API.Common.Models;

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
            var result = ValidateResult.Empty;
            if (string.IsNullOrEmpty(PersonName))
            {
                result.Messages.Add("`姓名`不可为空");
                return result;
            }
            if (Photo!=null&& Photo.Length>100)
            {
                result.Messages.Add("`照片内容`最大长度为100");
                return result;
            }
            return result;
        }
    }
}
