using MyShop.Data.Infrastructure;
using MyShop.Model.Models;

namespace MyShop.Data.Repositories
{
    public interface IProviderRepository : IRepository<Provider>
    {
    }
    public class ProviderRepository : RepositoryBase<Provider>, IProviderRepository
    {
        public ProviderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}