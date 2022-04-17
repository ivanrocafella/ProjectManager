using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectManager.Core;
using ProjectManager.Core.Repositories;
using ProjectManager.DAL.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly ConcurrentDictionary<Type, object> _repositories;
        private IDbContextTransaction _transaction;
        private bool _disposed;

        public IEmployeeRepository Employees { get; }
        public IProjectRepository Projects { get; }
        public IEmployeeProjectRepository EmployeeProjects { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<Type, object>();

            Employees = new EmployeeRepository(context);
            Projects = new ProjectRepository(context);
            EmployeeProjects = new EmployeeProjectRepository(context);
        }

        public void Complete() => _context.SaveChanges();

        public void BeginTransaction() => _transaction = _context.Database.BeginTransaction();

        public void BeginTransaction(IsolationLevel level) => _transaction = _context.Database.BeginTransaction(level);

        public void RollbackTransaction()
        {
            if (_transaction == null) return;

            _transaction.Rollback();
            _transaction.Dispose();

            _transaction = null;
        }

        public void CommitTransaction()
        {
            if (_transaction == null) return;

            _transaction.Commit();
            _transaction.Dispose();

            _transaction = null;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return _repositories.GetOrAdd(typeof(TEntity), new Repository<TEntity>(_context)) as IRepository<TEntity>;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _context.Dispose();

            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
