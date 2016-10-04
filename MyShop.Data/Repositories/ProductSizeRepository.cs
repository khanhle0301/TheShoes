using MyShop.Data.Infrastructure;
using MyShop.Model.Models;


namespace MyShop.Data.Repositories
{
    public interface IProductSizeRepository : IRepository<ProductSize>
    {
    }
    public class ProductSizeRepository : RepositoryBase<ProductSize>, IProductSizeRepository
    {
        public ProductSizeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}