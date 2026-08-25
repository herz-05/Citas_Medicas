using Core.Interface.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Repositories;

namespace Persistence
{
    public static class Extension
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            IConfiguration configuration;

            using (ServiceProvider provider = services.BuildServiceProvider())
                configuration = ServiceProviderServiceExtensions.GetService<IConfiguration>(provider)!;

            services.AddDbContext<ApplicationDbContext>(
                opt => opt.UseSqlServer(configuration["sql:cx"])
            );

            services.AddTransient<IPacientes, PacientesRepository>();

            services.AddTransient<IConsultorios, ConsultoriosRepository>();

            services.AddTransient<IHorariosMedicos, HorariosMedicosRepository>();

            return services;
        }
    }
}