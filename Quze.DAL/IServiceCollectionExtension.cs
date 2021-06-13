using Microsoft.Extensions.DependencyInjection;

namespace Quze.DAL
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddQuzeContext(this IServiceCollection services)
        {
        
   
            return services;
        }
    }

}
