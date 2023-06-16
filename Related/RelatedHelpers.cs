using Microsoft.Extensions.DependencyInjection;
using Related.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related
{
    public static class RelatedHelpers
    {
        public static IServiceCollection AddRelatedUI(this IServiceCollection services)
        {
            services.AddScoped<IRecomendationApiClient, RecomendationApiClient>();

            return services;
        }
    }
}
