using ProjectManager.Core.Entities;
using ProjectManager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.Repositories
{
    public class EmployeeProjectRepository : Repository<EmployeeProject>, IEmployeeProjectRepository
    {
        public EmployeeProjectRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
