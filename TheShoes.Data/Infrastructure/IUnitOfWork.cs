namespace TheShoes.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}