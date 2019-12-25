using System;
using System.Collections.Generic;

namespace VLTest2015.Service
{
    public class RoleService : IRoleService
    {
        public long CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public bool EditRoleAuthorities(long roleId, IEnumerable<long> authorityIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<long> GetRoleAuthorities(long roleId)
        {
            throw new NotImplementedException();
        }
    }
}
