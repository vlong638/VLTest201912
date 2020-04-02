namespace VL.API.Common.Models
{
    /// <summary>
    /// 数据校验
    /// </summary>
    public interface IDataValidation
    {
        public ValidateResult Validate();
    }
}
