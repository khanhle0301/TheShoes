using System;

namespace MyShop.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        MyShopDbContext Init();
    }
}