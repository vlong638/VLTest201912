using System.Data;

namespace VL.Consolo_Core.Common.DBSolution
{
    public class DbContext
    {
        public DbGroup DbGroup { set; get; }

        public DbContext()
        { }

        public DbContext(IDbConnection connection)
        {
            DbGroup = new DbGroup(connection);
        }
    }
}
