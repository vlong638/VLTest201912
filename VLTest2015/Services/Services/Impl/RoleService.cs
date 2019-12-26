using System.Collections.Generic;
using System.Linq;
using VLTest2015.DAL;

namespace VLTest2015.Services
{
    public class RoleService : BaseService, IRoleService
    {
        IRoleRepository _roleRepository;
        IRoleAuthorityRepository _roleAuthorityRepository;

        public RoleService()
        {
            _roleRepository = new RoleRepository(_connection);
            _roleAuthorityRepository = new RoleAuthorityRepository(_connection);
        }

        public ResponseResult<long> CreateRole(string roleName)
        {
            Role role = new Role()
            {
                Name = roleName,
            };
            var result = _roleRepository.GetBy(role.Name);
            if (result != null)
            {
                return new ResponseResult<long>()
                {
                    ErrorCode = -1,
                    ErrorMessage = "角色名称已存在",
                };
            }
            var id = _roleRepository.Insert(role);
            return new ResponseResult<long>(id);
        }

        public ResponseResult<bool> EditRoleAuthorities(long roleId, IEnumerable<long> authorityIds)
        {
            return _connection.DelegateTransaction(() =>
            {
                _roleAuthorityRepository.DeleteBy(roleId);
                var roleAuthorities = authorityIds.Select(c => new RoleAuthority() { RoleId = roleId, AuthorityId = c }).ToArray();
                _roleAuthorityRepository.Insert(roleAuthorities);
                return true;
            });
        }

        public ResponseResult<IEnumerable<long>> GetRoleAuthorityIds(long roleId)
        {
            var roleAuthorities = _roleAuthorityRepository.GetBy(roleId);
            return new ResponseResult<IEnumerable<long>>(roleAuthorities.Select(c => c.AuthorityId));
        }
    }
}
