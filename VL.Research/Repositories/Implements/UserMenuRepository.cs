using Dapper;
using System.Collections.Generic;
using System.Linq;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using BBee.Models;

namespace BBee.Repositories
{
    public class UserMenuRepository : Repository<UserMenu>
    {
        public UserMenuRepository(DbContext context) : base(context)
        {
        }

        internal List<UserMenu> GetByUserId(long userId)
        {
            return _connection.Query<UserMenu>("select * from [A_UserMenu] where UserId = @userId;"
                , new { userId }).ToList();
        }

        //public UserMenu GetBy(string name)
        //{
        //    return _connection.Query<UserMenu>("select * from [A_UserMenu] where Name = @name;"
        //        , new { name }).FirstOrDefault();
        //}

        //public IEnumerable<UserMenu> GetAll()
        //{
        //    return _connection.Query<UserMenu>("select * from [A_UserMenu] order by Id desc;");
        //}
    }
}