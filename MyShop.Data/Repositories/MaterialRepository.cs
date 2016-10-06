using MyShop.Data.Infrastructure;
using MyShop.Model.Models;

namespace MyShop.Data.Repositories
{
    public interface IMaterialRepository : IRepository<Material>
    {
    }
    public class MaterialRepository : RepositoryBase<Material>, IMaterialRepository
    {
        public MaterialRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
