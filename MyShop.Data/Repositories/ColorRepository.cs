using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyShop.Data.Repositories
{
    public interface IColorRepository : IRepository<Color>
    {
        IEnumerable<Color> GetListColorByProductId(int id);
    }
    public class ColorRepository : RepositoryBase<Color>, IColorRepository
    {
        public ColorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
          
        }
        public IEnumerable<Color> GetListColorByProductId(int id)
        {
            var query = from g in DbContext.Colors
                        join ug in DbContext.ProductColors
                        on g.ID equals ug.ColorID
                        where ug.ProductID == id
                        select g;
            return query;
        }
    }
}