using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role GetBy(string name);
    }
}