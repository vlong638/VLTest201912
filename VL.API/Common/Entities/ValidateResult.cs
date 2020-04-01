using System.Linq;

namespace VL.API.Common.Entities
{
    /// <summary>
    /// 数据校验结果集
    /// </summary>
    public class ValidateResult
    {
        public static ValidateResult Empty = new ValidateResult();

        public ValidateResult(params string[] messages)
        {
            this.Code = 0;
            this.Messages = messages;
        }
        public ValidateResult(int code = 0, params string[] messages)
        {
            this.Code = code;
            this.Messages = messages;
        }

        public int Code { set; get; }
        public string[] Messages { set; get; }
        public bool IsValidated { get { return Messages.Count() == 0; } }
    }
}
