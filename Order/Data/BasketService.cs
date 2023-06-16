using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Order.Data.IBasketService;

namespace Order.Data
{
    internal interface IBasketService
    {
        public Task AddAsync(int tractorId, int quantity);

        public int Count { get; }

        public event Action? OnChange;
    }

    internal interface IPriceApiClient
    {
        public Task<int> GetPriceAsync(int tractorId);
    }

    internal class BasketService : IBasketService
    {
        public int Count { get; private set; }

        public event Action? OnChange;

        public Task AddAsync(int tractorId, int quantity)
        {
            Count += quantity;
            OnChange?.Invoke();

            return Task.CompletedTask;
        }
    }

    internal class PriceApiClient : IPriceApiClient
    {
        public Task<int> GetPriceAsync(int tractorId)
        {
            var price = (float)tractorId * 1300;

            return Task.FromResult((int)price);
        }
    }
}
