using ProjectManager.Core;
using ProjectManager.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IApplicationDbContextFactory _applicationDbContextFactory;

        public UnitOfWorkFactory(IApplicationDbContextFactory applicationDbContextFactory)
        {
            _applicationDbContextFactory = applicationDbContextFactory;
        }
        public IUnitOfWork MakeUnitOfWork() => new UnitOfWork(_applicationDbContextFactory.Create());
    }
}
