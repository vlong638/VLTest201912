using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    /// <summary>
    /// 数据校验
    /// </summary>
    public interface IDataValidation
    {
        public ValidateResult Validate();
    }

    /// <summary>
    /// 数据校验结果集
    /// </summary>
    public class ValidateResult
    {
        public static ValidateResult Empty = new ValidateResult();

        public ValidateResult(params string[] messages)
        {
            Messages = messages;
        }

        public string[] Messages { set; get; }
        public bool IsValidated { get { return Messages.Count() == 0; } }
    }
}
