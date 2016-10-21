using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyShop.Data.Repositories
{
    public interface IHeightRepository : IRepository<Height>
    {
        IEnumerable<Height> GetListHeightByProductId(int id);
    }
    public class HeightRepository : RepositoryBase<Height>, IHeightRepository
    {
        public HeightRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public IEnumerable<Height> GetListHeightByProductId(int id)
        {
            var query = from g in DbContext.Heights
                        join ug in DbContext.ProductHeights
                        on g.ID equals ug.HeightId
                        where ug.ProductId == id
                        select g;
            return query;
        }
    }
}