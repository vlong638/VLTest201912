using System.Collections.Generic;
using VL.Consolo_Core.Common.PagerSolution;

namespace BBee.Models
{
    public class GetUserPageListRequest : PagerRequest
    {
        public string UserName { set; get; }

        public GetUserPageListRequest()
        {
            Page = 1;
            Rows = 20;
        }


        public string GetWhereCondition()
        {
            return string.IsNullOrEmpty(UserName) ? "" : " Name like @UserName ";
        }

        public override Dictionary<string, object> GetParameters()
        {
            return new Dictionary<string, object>()
            {
                { nameof(UserName),$"%{UserName}%"}
            };
        }
    }
}