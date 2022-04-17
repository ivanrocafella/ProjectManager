using Microsoft.EntityFrameworkCore;
using ProjectManager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ApplicationDbContext Context;
        protected DbSet<TEntity> DbSet;
        private bool _disposed;

        public Repository(ApplicationDbContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public virtual void Add(TEntity obj) => DbSet.Add(obj);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                Context.Dispose();

            _disposed = true;
        }

        ~Repository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => DbSet.Where(predicate);

        public virtual IQueryable<TEntity> GetAll() => DbSet;

        public virtual TEntity GetById(int id) => DbSet.Find(id);

        public virtual void Remove(int id) => DbSet.Remove(DbSet.Find(id));

        public void RemoveByEntity(TEntity entity) => DbSet.Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);

        public virtual void Update(TEntity obj) => DbSet.Update(obj);

    }
}
