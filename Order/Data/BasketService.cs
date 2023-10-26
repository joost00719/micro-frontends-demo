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

        public Task<int> GetCount();

        public event HandleChange OnChange;

        public delegate void HandleChange();
    }

    internal interface IPriceApiClient
    {
        public Task<int> GetPriceAsync(int tractorId);
    }

    internal class BasketService : IBasketService
    {
        private int _count = 0;

        public event HandleChange OnChange;

        public Task AddAsync(int tractorId, int quantity)
        {
            _count += quantity;
            OnChange?.Invoke();

            return Task.CompletedTask;
        }

        public Task<int> GetCount()
        {
            return Task.FromResult(_count);
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
