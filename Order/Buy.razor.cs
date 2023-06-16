using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Order.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Order
{
    public partial class Buy
    {
        [Inject] private IPriceApiClient _priceApiClient { get; set; }
        [Inject] private IBasketService _basketService { get; set; }

        private bool _loaded;
        private int _price;

        [Parameter]
        public int? TractorId { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            _loaded = false;
            _price = TractorId != null
                ? await _priceApiClient.GetPriceAsync(TractorId.Value)
                : 0;
            _loaded = true;
        }

        private async Task AddToBasket()
        {
              await _basketService.AddAsync(TractorId.Value, 1);
        }
    }
}
