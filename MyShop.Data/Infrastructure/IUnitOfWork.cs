namespace MyShop.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}