using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MyShop.Data.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow);
    }

    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Post> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow)
        {
            var query = from a in DbContext.Posts
                        join b in DbContext.PostTags
                        on a.ID equals b.PostID
                        join c in DbContext.PostCategories
                        on a.CategoryID equals c.ID
                        where b.TagID == tag && a.Status
                        orderby a.CreatedDate descending
                        select a;
            var model = query.Include("PostCategory");
            totalRow = model.Count();
            return model.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}