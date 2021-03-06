﻿using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VLTest2015.Services;
using System;

namespace VLTest2015.DAL
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<UserRoleInfo> GetUserRoleInfosBy(long[] userIds)
        {
            return _connection.Query<UserRoleInfo>(@"select ur.UserId,ur.RoleId,r.Name as RoleName
from [A_Role] r
left join [A_UserRole] ur on ur.RoleId = r.Id
where ur.UserId in @userIds;"
                , new { userIds });
        }

        public Role GetBy(string name)
        {
            return _connection.Query<Role>("select * from [A_Role] where Name = @name;"
                , new { name }).FirstOrDefault();
        }

        public IEnumerable<Role> GetAll()
        {
            return _connection.Query<Role>("select * from [A_Role] order by Id desc;");
        }
    }
}