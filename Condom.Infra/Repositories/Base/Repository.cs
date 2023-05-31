using Condom.Domain.Global;
using Condom.Domain.Models.Identity;
using Condom.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Condom.Infra.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task SaveChanges(CancellationToken cancellationToken);
        void Dispose();
        void ThrowIfDisposed();
        Task<TEntity> CreateAsync(TEntity domain);
        Task<Tracker> SaveChangeAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> where);
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

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            return await OwnDbSet.CountAsync(where);
        }

        public async Task<TEntity> CreateAsync(TEntity domain)
        {
            await _Context.AddAsync(domain);
            return domain;
        }

        public async Task<Tracker> SaveChangeAsync()
        {
            Tracker t = new Tracker();
            var i = await Context.SaveChangesAsync(new CancellationToken());
            if (i > 0)
            {
                t.AddLog(MessageTypeEnum.Success, "Operação finalizada com sucesso");
            }
            else
            {
                t.AddLog(MessageTypeEnum.Error, "Operação finalizada com erro!");
            }
            return t;
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
