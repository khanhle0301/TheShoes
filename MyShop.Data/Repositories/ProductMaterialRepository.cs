using MyShop.Data.Infrastructure;
using MyShop.Model.Models;

namespace MyShop.Data.Repositories
{
    public interface IProductMaterialRepository : IRepository<ProductMaterial>
    {
    }

    public class ProductMaterialRepository : RepositoryBase<ProductMaterial>, IProductMaterialRepository
    {
        public ProductMaterialRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}