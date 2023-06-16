using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Related.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Related
{
    public partial class Recos
    {
        [Inject] private IRecomendationApiClient _recomendationApiClient { get; set; }

        private bool _loaded;
        private Recommendation _recommendation;

        [Parameter]
        public int? TractorId { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            _loaded = false; // in case of reload
            _recommendation = TractorId != null 
                ? await _recomendationApiClient.GetRecommendationAsync(TractorId.Value) 
                : null;
            _loaded = true;
        }
    }
}
