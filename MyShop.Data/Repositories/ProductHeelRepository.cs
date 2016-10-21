using MyShop.Data.Infrastructure;
using MyShop.Model.Models;

namespace MyShop.Data.Repositories
{
    public interface IProductHeelRepository : IRepository<ProductHeel>
    {
    }
    public class ProductHeelRepository : RepositoryBase<ProductHeel>, IProductHeelRepository
    {
        public ProductHeelRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}