using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyShop.Data.Repositories
{
    public interface IMaterialRepository : IRepository<Material>
    {
        IEnumerable<Material> GetListMaterialByProductId(int id);
    }
    public class MaterialRepository : RepositoryBase<Material>, IMaterialRepository
    {
        public MaterialRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public IEnumerable<Material> GetListMaterialByProductId(int id)
        {
            var query = from g in DbContext.Materials
                        join ug in DbContext.ProductMaterials
                        on g.ID equals ug.MaterialID
                        where ug.ProductID == id
                        select g;
            return query;         
        }
    }
}
