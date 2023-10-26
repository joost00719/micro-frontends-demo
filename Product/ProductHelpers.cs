﻿using Microsoft.Extensions.DependencyInjection;
using Order;
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
        /// <summary>
        /// Adds the required services for the Product UI to function.
        /// </summary>
        /// <param name="services">The service collection to register the services to.</param>
        public static IServiceCollection AddProductUI(this IServiceCollection services)
        {
            services.AddScoped<ITractorApiClient, TractorApiClient>();
            services.AddRelatedUI(); // This application uses Realted components
            services.AddOrderUI(); // This application uses Order components

            return services;
        }
    }
}
