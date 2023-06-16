using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Data
{
    internal interface IRecomendationApiClient
    {
        public Task<Recommendation> GetRecommendationAsync(int tractorId);
    }
}
