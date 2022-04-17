using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.Contracts
{
    public interface IApplicationDbContextFactory
    {
        ApplicationDbContext Create();
    }
}
