using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ServiceSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountService
    {
        APIContext APIContext { get; set; }
        DbContext ResearchDbContext { set; get; }

        UserRepository UserRepository { set; get; }
        UserRoleRepository UserRoleRepository { set; get; }
        RoleRepository RoleRepository { set; get; }

        /// <summary>
        /// 
        /// </summary>
        internal AccountService(DbContext DbContext)
        {
            ResearchDbContext = DbContext;
            Init(DbContext);
        }
        /// <summary>
        /// 
        /// </summary>
        public AccountService(APIContext apiContext)
        {
            APIContext = apiContext;
            ResearchDbContext = APIContext?.GetDBContext(APIContraints.ResearchDbContext);
            Init(ResearchDbContext);
        }

        private void Init(DbContext DbContext)
        {
            //repositories
            UserRepository = new UserRepository(DbContext);
            UserRoleRepository = new UserRoleRepository(DbContext);
            RoleRepository = new RoleRepository(DbContext);
        }

        internal ServiceResult<User> PasswordSignIn(string userName, string password)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var user = UserRepository.GetByUserNameAndPassword(userName, password);
                if (user==null)
                {
                    throw new NotImplementedException("用户不存在或密码错误");
                }
                return user;
            });
        }

        internal ServiceResult<Dictionary<long, string>> GetAllRolesIdAndName()
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var result = RoleRepository.GetAllSystemRoles();
                return result.ToDictionary(c => c.Id, c => c.Name);
            });
        }

        internal ServiceResult<VLPagerResult<List<GetUserModel>>> GetPagedUsers(int page,int limit,string username,string nickname)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var list = UserRepository.GetPagedUsers(page, limit, username, nickname).Select(c => new GetUserModel()
                {
                    UserId = c.Id,
                    UserName = c.Name,
                    NickName = c.NickName,
                    IsDeleted = c.IsDeleted,
                }).ToList();
                var userRoles = RoleRepository.GetSystemRolesByUserIds(list.Select(c => c.UserId).ToList());
                list.ForEach(c =>
                {
                    c.Roles = userRoles.Where(ur => ur.UserId == c.UserId).Select(ur => new GetUserRoleModel() { RoleId = ur.RoleId, RoleName = ur.RoleName }).ToList();
                });
                var count = UserRepository.GetPagedUserCount(username, nickname);
                return new VLPagerResult<List<GetUserModel>>() { List = list, Count = count };
            });
        }

        internal ServiceResult<long> CreateUser(User user, List<long> roleIds)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                user.CreatedAt = DateTime.Now;
                var repeat = UserRepository.GetByUserName(user.Name);
                if (repeat != null)
                { 
                    throw new NotImplementedException("用户名已存在");
                }
                var id = UserRepository.InsertOne(user);
                var userRoles = roleIds.Select(c => new UserRole() { UserId = id, RoleId = c });
                foreach (var userRole in userRoles)
                {
                    UserRoleRepository.Insert(userRole);
                }
                return id;
            });
        }

        internal ServiceResult<bool> LogicDeleteUser(long userId)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var result = UserRepository.UpdateUserStatus(userId, IsDeleted: true);
                return result > 0;
            });
        }

        internal ServiceResult<bool> LogicUndoDeleteUser(long userId)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var result = UserRepository.UpdateUserStatus(userId, IsDeleted: true);
                return result > 0;
            });
        }

        internal ServiceResult<bool> EditUser(User newUser, List<long> roleIds)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                newUser.CreatedAt = DateTime.Now;
                var user = UserRepository.GetByUserName(newUser.Name);
                user.NickName = newUser.NickName;
                user.Sex = newUser.Sex;
                user.Phone = newUser.Phone;
                UserRepository.Update(user);
                UserRoleRepository.DeleteByUserId(user.Id);
                var userRoles = roleIds.Select(c => new UserRole() { UserId = newUser.Id, RoleId = c });
                foreach (var userRole in userRoles)
                {
                    UserRoleRepository.Insert(userRole);
                }
                return true;
            });
        }
    }
}
