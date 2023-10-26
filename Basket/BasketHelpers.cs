using Basket.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket
{
    public static class BasketHelpers
    {
        public static IServiceCollection AddBasketUI(this IServiceCollection services)
        {
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IPriceService, PriceService>();

            return services;
        }
    }
}
