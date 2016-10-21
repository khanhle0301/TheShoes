using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyShop.Data.Repositories
{
    public interface ISizeRepository : IRepository<Size>
    {
        IEnumerable<Size> GetListSizeByProductId(int id);
    }
    public class SizeRepository : RepositoryBase<Size>, ISizeRepository
    {
        public SizeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public IEnumerable<Size> GetListSizeByProductId(int id)
        {
            var query = from g in DbContext.Sizes
                        join ug in DbContext.ProductSizes
                        on g.ID equals ug.SizeID
                        where ug.ProductID == id
                        select g;
            return query;
        }
    }
}