using Microsoft.AspNetCore.Components;
using Order.Data;


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
