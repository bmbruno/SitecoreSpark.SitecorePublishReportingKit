using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using SitecoreSpark.SPRK.Controllers;
using SitecoreSpark.SPRK.Implementation;
using SitecoreSpark.SPRK.Interfaces;
using SitecoreSpark.SPRK.Models;

namespace SitecoreSpark.SPRK
{
    public class SparkConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            //
            // TODO: Review these registrations to ensure they are the correct types that I intended
            //

            // Controllers
            serviceCollection.AddTransient<ReportController>();

            // Services / Managers
            serviceCollection.AddSingleton<ISparkLogger, SparkLogger>();
            serviceCollection.AddScoped<ILogManager<LogItem>, LogManager>();
        }
    }
}