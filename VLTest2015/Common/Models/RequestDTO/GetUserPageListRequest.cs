using System.Collections.Generic;

namespace VLTest2015.Services
{
    public class GetUserPageListRequest : PagerRequest
    {
        public string UserName { set; get; }

        public GetUserPageListRequest()
        {
            Page = 1;
            Rows = 20;
        }


        public override string GetWhereCondition()
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