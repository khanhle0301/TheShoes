using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyShop.Data.Repositories
{
    public interface ISlideRepository : IRepository<Slide>
    {
        IEnumerable<Slide> Check(string name);
    }

    public class SlideRepository : RepositoryBase<Slide>, ISlideRepository
    {
        public SlideRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public IEnumerable<Slide> Check(string name)
        {
            var query = from x in DbContext.Slides
                        where x.Name == name
                        orderby x.ID descending
                        select x;
            return query;
        }
    }
}