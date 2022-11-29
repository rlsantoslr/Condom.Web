using Condom.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task SaveChanges(CancellationToken cancellationToken);
        void Dispose();
        void ThrowIfDisposed();
    }

    public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
    {
        readonly CondomContext _Context;

        public CondomContext Context { get => _Context; }

        public bool AutoSaveChanges { get; set; } = true;

        private bool _disposed;

        public readonly DbSet<TEntity> OwnDbSet;

        public Repository(CondomContext context)
        {
            _Context = context ?? throw new ArgumentNullException(nameof(context));
            OwnDbSet = Context.Set<TEntity>();
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            if (AutoSaveChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
        }

        public void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
