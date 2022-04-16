using BackendProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewModels
{
    public class EventVM
    {
        public Event Event { get; set; }
        public List<Speakers> Speakers { get; set; }
        public List<Category> Categories { get; set; }
    }
}
