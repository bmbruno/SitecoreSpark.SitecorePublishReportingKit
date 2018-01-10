using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using SitecoreSpark.SPRK.Interfaces;

namespace SitecoreSpark.SPRK
{
    public class SparkConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISparkLogger, SparkLogger>();
        }
    }
}