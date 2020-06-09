using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VLTest2015.Services;
using System;

namespace VLTest2015.DAL
{
    public class UserMenuRepository : Repository<UserMenu>
    {
        public UserMenuRepository(DbContext context) : base(context)
        {
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