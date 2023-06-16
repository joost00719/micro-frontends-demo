using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Order.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Order
{
    public partial class Basket : IDisposable
    {
        [Inject] IBasketService _basketService { get; set; }

        private int _count;

        private Task<int> Getcount()
        {
            return _basketService.GetCount();
        }

        protected override void OnInitialized()
        {
            _basketService.OnChange += CountChanged;
            CountChanged();
        }

        private async void CountChanged()
        {
            _count = await _basketService.GetCount();
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _basketService.OnChange -= CountChanged;
        }
    }
}
