using Microsoft.AspNetCore.Components;
using Product.Data;


namespace Product
{
    public partial class App
    {
        private IEnumerable<Tractor> _tractors;
        private Tractor _selectedTractor;
        bool _loaded = false;

        [Inject] private ITractorApiClient _tractorService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            _loaded = false; // in case of reload
            _tractors = await _tractorService.GetTractorsAsync();
            _selectedTractor = _tractors.FirstOrDefault();
            _loaded = true;
        }

        private void tractorClicked(Tractor tractor)
        {
            _selectedTractor = tractor;
            InvokeAsync(StateHasChanged);
        }
    }
}
