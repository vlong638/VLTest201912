namespace VL.API.Common.Repositories
{
    public class DbRepository
    {
        protected DbGroup dbGroup;

        public DbRepository(DbGroup dbGroup)
        {
            this.dbGroup = dbGroup;
        }
    }
}
