using Autobots.Common.ServiceBase;
using Autobots.EMRServices.DBSolution;
using System;

namespace Autobots.EMRServices.Repositories
{
    public class SharedRepository : IRepository<object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SharedRepository(DbContext context)
        {
            Context = context;
        }

        public DbContext Context { get; private set; }

        public bool Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteById(long id)
        {
            throw new NotImplementedException();
        }

        public object GetById(long id)
        {
            throw new NotImplementedException();
        }

        public long Insert(object entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(object entity)
        {
            throw new NotImplementedException();
        }
    }
}