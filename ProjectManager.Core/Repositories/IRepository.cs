using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Core.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        TEntity GetById(int id);
        IQueryable<TEntity> GetAll();
        void Update(TEntity obj);
        void Remove(int id);
        void RemoveByEntity(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    }
}
