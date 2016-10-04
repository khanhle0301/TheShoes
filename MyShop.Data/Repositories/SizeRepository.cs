using MyShop.Data.Infrastructure;
using MyShop.Model.Models;


namespace MyShop.Data.Repositories
{
    public interface ISizeRepository : IRepository<Size>
    {
    }
    public class SizeRepository : RepositoryBase<Size>, ISizeRepository
    {
        public SizeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}