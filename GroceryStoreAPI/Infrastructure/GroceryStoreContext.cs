using GroceryStoreAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.EntityFrameworkCore.Design;

namespace GroceryStoreAPI.Infrastructure
{
    public class GroceryStoreContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "customer";
        public DbSet<Customer> Customers { get; set; }


        private IDbContextTransaction _currentTransaction;
        public GroceryStoreContext(DbContextOptions<GroceryStoreContext> options) : base(options) { }
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
        }
        public async Task<bool> SaveEntitiesAsync(System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }

    public class CustomerContextDesignFactory : IDesignTimeDbContextFactory<GroceryStoreContext>
    {
        public GroceryStoreContext CreateDbContext(string[] args)
        {
            //TODO : update db
            var optionsBuilder = new DbContextOptionsBuilder<GroceryStoreContext>()
                .UseSqlServer("Server=.;Initial Catalog=GroceryStoreAPI;Integrated Security=true");
            return new GroceryStoreContext(optionsBuilder.Options);
        }
    }

}
