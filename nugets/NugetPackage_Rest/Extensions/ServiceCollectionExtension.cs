using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NugetPackage_Rest.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetPackage_Rest.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>Enlaza la sección "RestSettings" del appsettings.json con
        /// IOptions&lt;RequestSettings&gt;, que RestBuilder recibe por constructor.</summary>
        public static IServiceCollection AddRequestLogging(this IServiceCollection services,
        IConfiguration configuration)
        {
            services.Configure<RequestSettings>(configuration.GetSection("RestSettings"));

            return services;
        }
    }
}
