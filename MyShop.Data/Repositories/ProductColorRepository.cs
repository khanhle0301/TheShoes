using MyShop.Data.Infrastructure;
using MyShop.Model.Models;

namespace MyShop.Data.Repositories
{
    public interface IProductColorRepository : IRepository<ProductColor>
    {
    }
    public class ProductColorRepository : RepositoryBase<ProductColor>, IProductColorRepository
    {
        public ProductColorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}