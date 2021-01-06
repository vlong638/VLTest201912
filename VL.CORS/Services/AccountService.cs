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
            RoleRepository = new RoleRepository(DbContext);
        }

        internal ServiceResult<User> PasswordSignIn(string userName, string password)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var user = UserRepository.GetByUserNameAndPassword(userName, password.ToMD5());
                if (user==null)
                {
                    throw new NotImplementedException("用户不存在或密码错误");
                }
                return user;
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
    }
}
