using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandAloneService.Pages
{
    public partial class TestPage
    {
        private int _count;

        void button_pressed()
        {
            _count++;
        }
    }
}
