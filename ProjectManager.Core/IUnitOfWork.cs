using ProjectManager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository Employees { get; }
        IProjectRepository Projects { get; }
        IEmployeeProjectRepository EmployeeProjects { get; }

        void Complete();
        void BeginTransaction();
        void BeginTransaction(IsolationLevel level);
        void RollbackTransaction();
        void CommitTransaction();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
