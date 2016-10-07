using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data.Entity;

namespace MyShop.Data.Repositories
{ 
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAllByTag(string tagId);
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Product> GetAllByTag(string tagId)
        {
            var query = from a in DbContext.Products
                        join b in DbContext.ProductTags
                        on a.ID equals b.ProductID                       
                        where b.TagID == tagId && a.Status                       
                        select a;
            return query.Include("ProductCategory");           
        }
    }
}