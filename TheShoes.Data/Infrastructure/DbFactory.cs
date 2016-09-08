namespace TheShoes.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private TheShoesDbContext dbContext;

        public TheShoesDbContext Init()
        {
            return dbContext ?? (dbContext = new TheShoesDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}