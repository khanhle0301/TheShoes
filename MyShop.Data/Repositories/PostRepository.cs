using MyShop.Common.ViewModel;
using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyShop.Data.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<PostViewModel> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow);
    }

    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<PostViewModel> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow)
        {
            var query = (from a in DbContext.Posts
                         join b in DbContext.PostTags
                         on a.ID equals b.PostID
                         join c in DbContext.PostCategories
                        on a.CategoryID equals c.ID
                         where b.TagID == tag && a.Status
                         orderby a.CreatedDate descending
                         select new
                         {
                             ID = a.ID,
                             Name = a.Name,
                             Alias = a.Alias,
                             CategoryID = a.CategoryID,
                             CategoryAlias = c.Alias,
                             Image = a.Image,
                             Description = a.Description,
                             Content = a.Content,
                             HomeFlag = a.HomeFlag,
                             HotFlag = a.HotFlag,
                             ViewCount = a.ViewCount,
                             CreatedDate = a.CreatedDate,
                             CreatedBy = a.CreatedBy,
                             UpdatedDate = a.UpdatedDate,
                             UpdatedBy = a.UpdatedBy,
                             MetaKeyword = a.MetaKeyword,
                             MetaDescription = a.MetaDescription,
                             Status = a.Status,
                             Tags = a.Tags
                         }).AsEnumerable().Select(x => new PostViewModel()
                         {
                             ID = x.ID,
                             Name = x.Name,
                             Alias = x.Alias,
                             CategoryID = x.CategoryID,
                             CategoryAlias = x.CategoryAlias,
                             Image = x.Image,
                             Description = x.Description,
                             Content = x.Content,
                             HomeFlag = x.HomeFlag,
                             HotFlag = x.HotFlag,
                             ViewCount = x.ViewCount,
                             CreatedDate = x.CreatedDate,
                             CreatedBy = x.CreatedBy,
                             UpdatedDate = x.UpdatedDate,
                             UpdatedBy = x.UpdatedBy,
                             MetaKeyword = x.MetaKeyword,
                             MetaDescription = x.MetaDescription,
                             Status = x.Status,
                             Tags = x.Tags
                         });
            totalRow = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}