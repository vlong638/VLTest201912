using System.Collections.Generic;
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
            if (messages != null & messages.Length != 0)
                this.Messages.AddRange(messages);
        }
        public ValidateResult(int code = 0, params string[] messages)
        {
            this.Code = code;
            if (messages != null & messages.Length != 0)
                this.Messages.AddRange(messages);
        }

        public int Code { set; get; }
        public List<string> Messages { set; get; } = new List<string>();
        public bool IsValidated { get { return Messages.Count() == 0; } }
    }
}
