namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateResult
    {
        public ValidateResult(ValidateResultCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public ValidateResultCode Code { set; get; }
        public string Message { set; get; }
    }

    public enum ValidateResultCode
    {
        None = 0,
        Success =1,
    }
}
