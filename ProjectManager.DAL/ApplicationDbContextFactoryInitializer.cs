using Microsoft.EntityFrameworkCore;
using ProjectManager.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL
{
    public static class ApplicationDbContextFactoryInitializer
    {
        public static IApplicationDbContextFactory Create(string dbConnectionString)
        {
            if (string.IsNullOrWhiteSpace(dbConnectionString))
                throw new ArgumentNullException(nameof(dbConnectionString));

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(dbConnectionString);
            return new ApplicationDbContextFactory(optionsBuilder.Options);
        }
    }
}
