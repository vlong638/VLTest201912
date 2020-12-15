using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchAPI.CORS.Common
{
    public class UserFavoriteProject
    {
        public long UserId { set; get; }
        public long ProjectId { set; get; }
    }
}
