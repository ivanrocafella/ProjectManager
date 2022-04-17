using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Core;
using ProjectManager.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL.Extensions
{
    public static class AppDalConfiguringExtensions
    {
        public static void AddDataBase(this IServiceCollection services, IApplicationDbContextFactory applicationDbContextFactory)
        {
            services.AddScoped(sp => applicationDbContextFactory.Create());

            services.AddDbContext<ApplicationDbContext>();

            services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddSingleton(sp => applicationDbContextFactory);
        }
    }
}
