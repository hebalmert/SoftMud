using Microsoft.EntityFrameworkCore.Storage;
using Spix.Infrastructure;

namespace Spix.Helper.Transactions;

public class TransactionManager : ITransactionManager
{
    private readonly DataContext _context;
    private IDbContextTransaction? _transaction;

    public TransactionManager(DataContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.SaveChangesAsync();
        await _transaction!.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction!.RollbackAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}