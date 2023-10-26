using Microsoft.Extensions.DependencyInjection;
using Order.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order
{
    public static class OrderHelpers
    {
        public static IServiceCollection AddOrderUI(this IServiceCollection services)
        {
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IPriceApiClient, PriceApiClient>();

            return services;
        }
    }
}
