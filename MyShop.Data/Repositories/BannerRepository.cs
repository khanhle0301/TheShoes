using MyShop.Data.Infrastructure;
using MyShop.Model.Models;

namespace MyShop.Data.Repositories
{
    public interface IBannerRepository : IRepository<Banner>
    {
    }
    public class BannerRepository : RepositoryBase<Banner>, IBannerRepository
    {
        public BannerRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}