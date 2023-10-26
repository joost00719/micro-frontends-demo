using Microsoft.Extensions.DependencyInjection;
using Product.Data;
using Related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Product
{
    public static class ProductHelpers
    {
        public static IServiceCollection AddProductUI(this IServiceCollection services)
        {
            services.AddScoped<ITractorApiClient, TractorApiClient>();
            services.AddRelatedUI();

            return services;
        }
    }
}
