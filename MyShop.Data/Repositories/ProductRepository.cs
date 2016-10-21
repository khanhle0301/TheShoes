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

        IEnumerable<Product> GetProductColor(IEnumerable<Product> Products, int id);

        IEnumerable<Product> GetProductType(IEnumerable<Product> Products, int id);

        IEnumerable<Product> GetProductMaterial(IEnumerable<Product> Products, int id);

        IEnumerable<Product> GetProductHeel(IEnumerable<Product> Products, int id);

        IEnumerable<Product> GetProductHeight(IEnumerable<Product> Products, int id);
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

        public IEnumerable<Product> GetProductColor(IEnumerable<Product> Products, int id)
        {
            var query = from a in Products
                        join b in DbContext.ProductColors
                        on a.ID equals b.ProductID
                        where b.ColorID == id && a.Status
                        select a;
            return query;
        }

        public IEnumerable<Product> GetProductHeel(IEnumerable<Product> Products, int id)
        {
            var query = from a in Products
                        join b in DbContext.ProductHeels
                        on a.ID equals b.ProductId
                        where b.HeelId == id && a.Status
                        select a;
            return query;
        }


        public IEnumerable<Product> GetProductHeight(IEnumerable<Product> Products, int id)
        {
            var query = from a in Products
                        join b in DbContext.ProductHeights
                        on a.ID equals b.ProductId
                        where b.HeightId == id && a.Status
                        select a;
            return query;
        }

        public IEnumerable<Product> GetProductMaterial(IEnumerable<Product> Products, int id)
        {
            var query = from a in Products
                        join b in DbContext.ProductMaterials
                        on a.ID equals b.ProductID
                        where b.MaterialID == id && a.Status
                        select a;
            return query;
        }

        public IEnumerable<Product> GetProductType(IEnumerable<Product> Products, int id)
        {
            var query = from a in Products
                        join b in DbContext.ProductTypes
                        on a.ID equals b.ProductId
                        where b.TypeId == id && a.Status
                        select a;
            return query;
        }
    }
}