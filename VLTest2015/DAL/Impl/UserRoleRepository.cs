﻿using Dapper;
using System.Collections.Generic;
using System.Data;
using VLTest2015.Services;
using System;

namespace VLTest2015.DAL
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbConnection connection) : base(connection)
        {
        }

        public int DeleteBy(long userId)
        {
            return _connection.Execute("Delete from [UserRole] where UserId = @UserId;"
                , new { UserId = userId });
        }

        public IEnumerable<UserRole> GetBy(long userId)
        {
            return _connection.Query<UserRole>("select * from [UserRole] where UserId = @UserId;"
                , new { UserId = userId });
        }
    }
}