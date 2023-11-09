using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Service to demonstrate registration in the DI container.
    /// </summary>
    public class CounterService
    {
        public int Count { get; private set; }

        public void Increment()
        {
            Count++;
        }
    }
}
