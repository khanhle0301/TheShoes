using MyShop.Data.Infrastructure;
using MyShop.Model.Models;

namespace MyShop.Data.Repositories
{
    public interface IVendorRepository : IRepository<Vendor>
    {
    }
    public class VendorRepository : RepositoryBase<Vendor>, IVendorRepository
    {
        public VendorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}