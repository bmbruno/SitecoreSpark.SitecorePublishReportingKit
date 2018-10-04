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
            // Controllers
            serviceCollection.AddTransient<ReportController>();

            // Services / Managers
            serviceCollection.AddSingleton<ISparkLogger, SparkLogger>();
            serviceCollection.AddTransient<ILogManager<LogItem>, LogManager>();
        }
    }
}