using Autobots.Infrastracture.Common.DBSolution;

namespace ResearchAPI.CORS.Repositories
{
    public class SharedRepository 
    {
        public SharedRepository(DbContext context)
        {
        }

        //public IEnumerable<LabCheck> GetAll()
        //{
        //    return context.DbGroup.Connection.Query<LabCheck>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        //}
    }
}