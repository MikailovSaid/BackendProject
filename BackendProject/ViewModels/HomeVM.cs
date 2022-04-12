using BackendProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewModels
{
    public class HomeVM
    {
        public List<Sliders> Sliders { get; set; }
        public List<ServiceArea> ServiceAreas { get; set; }

    }
}
