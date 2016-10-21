using MyShop.Data.Infrastructure;
using MyShop.Model.Models;

namespace MyShop.Data.Repositories
{
    public interface IProductHeightRepository : IRepository<ProductHeight>
    {
    }

    internal class ProductHeightRepository : RepositoryBase<ProductHeight>, IProductHeightRepository
    {
        public ProductHeightRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}