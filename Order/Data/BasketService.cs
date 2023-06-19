using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Data
{

    internal class BasketService : IBasketService
    {
        public int Count { get; private set; }

        public event Action? OnChange;

        public Task AddAsync(int tractorId, int quantity)
        {
            // In a real world scenario you'll want to implement an actual basket.
            // For this demo we only implemented the count, and don't care much about the actual items.

            Count += quantity;
            OnChange?.Invoke();

            return Task.CompletedTask;
        }
    }
}
