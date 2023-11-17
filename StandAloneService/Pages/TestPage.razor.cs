using Microsoft.AspNetCore.Components;
using Shared;

namespace StandAloneService.Pages
{
    public partial class TestPage
    {
        private int _count => counterService?.Count ?? 0;

        [Inject]
        private CounterService counterService { get; set; }

        void button_pressed()
        {
            counterService.Increment();
        }
    }
}
