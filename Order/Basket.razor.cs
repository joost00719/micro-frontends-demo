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

        private int _count => _basketService.Count;

        protected override void OnInitialized()
        {
            _basketService.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            _basketService.OnChange -= StateHasChanged;
        }
    }
}
