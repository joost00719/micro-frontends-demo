using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Data
{
    // fake api "client". In practice this will fetch from some external service
    internal class TractorApiClient : ITractorApiClient
    {
        // Get all tractors
        public async Task<IEnumerable<Tractor>> GetTractorsAsync()
        {
            await Task.Delay(1000);
            return new List<Tractor>
            {
                new Tractor { Id = 1, Name = "Tractor Porsche-Diesel Master 419", Price = 50000, Image =  "tractor-red.jpg"  },
                new Tractor { Id = 2, Name = "Tractor Fendt F20 Dieselroß", Price = 100000, Image = "tractor-green.jpg" },
                new Tractor { Id = 3, Name = "Tractor Eicher Diesel 215/16", Price = 200000, Image = "tractor-blue.jpg" }
            };
        }
    }
}
