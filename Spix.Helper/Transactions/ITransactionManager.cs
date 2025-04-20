namespace Spix.Helper.Transactions;

public interface ITransactionManager : IDisposable
{
    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();

    Task<int> SaveChangesAsync();
}