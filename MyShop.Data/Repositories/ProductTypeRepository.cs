using MyShop.Data.Infrastructure;
using MyShop.Model.Models;

namespace MyShop.Data.Repositories
{
    public interface IProductTypeRepository : IRepository<ProductType>
    {
    }
    class ProductTypeRepository : RepositoryBase<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}