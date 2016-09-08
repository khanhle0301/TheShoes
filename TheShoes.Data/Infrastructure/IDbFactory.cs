using System;

namespace TheShoes.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        TheShoesDbContext Init();
    }
}