using Microsoft.EntityFrameworkCore;
using ProjectManager.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL
{
    public class ApplicationDbContextFactory : IApplicationDbContextFactory
    {
        private readonly DbContextOptions _options;

        public ApplicationDbContextFactory(DbContextOptions options)
        {
            _options = options;
        }

        public ApplicationDbContext Create()
        {
            return new ApplicationDbContext(_options);
        }
    }
}
