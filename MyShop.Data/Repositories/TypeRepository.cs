using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyShop.Data.Repositories
{
    public interface ITypeRepository : IRepository<Type>
    {
        IEnumerable<Type> GetListTypeByProductId(int id);
    }
    public class TypeRepository : RepositoryBase<Type>, ITypeRepository
    {
        public TypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public IEnumerable<Type> GetListTypeByProductId(int id)
        {
            var query = from g in DbContext.Types
                        join ug in DbContext.ProductTypes
                        on g.ID equals ug.TypeId
                        where ug.ProductId == id
                        select g;
            return query;
        }
    }
}
