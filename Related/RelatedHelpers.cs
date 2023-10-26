using Microsoft.Extensions.DependencyInjection;
using Related.Data;

namespace Related
{
    public static class RelatedHelpers
    {
        /// <summary>
        /// Adds the required services for the Related UI to function.
        /// </summary>
        /// <param name="services">The service collection to register the services to.</param>
        public static IServiceCollection AddRelatedUI(this IServiceCollection services)
        {
            services.AddScoped<IRecomendationApiClient, RecomendationApiClient>();

            return services;
        }
    }
}
