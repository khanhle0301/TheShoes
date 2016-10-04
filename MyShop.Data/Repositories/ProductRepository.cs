using MyShop.Data.Infrastructure;
using MyShop.Model.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MyShop.Data.Repositories
{ 
    public interface IProductRepository : IRepository<Product>
    {
       
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

      
    }
}