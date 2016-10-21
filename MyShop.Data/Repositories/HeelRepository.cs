using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyShop.Data.Repositories
{
    public interface IHeelRepository : IRepository<Heel>
    {
        IEnumerable<Heel> GetListHeelByProductId(int id);
    }
    public class HeelRepository : RepositoryBase<Heel>, IHeelRepository
    {
        public HeelRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public IEnumerable<Heel> GetListHeelByProductId(int id)
        {
            var query = from g in DbContext.Heels
                        join ug in DbContext.ProductHeels
                        on g.ID equals ug.HeelId
                        where ug.ProductId == id
                        select g;
            return query;
        }
    }
}
