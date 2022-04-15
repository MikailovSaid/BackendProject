using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Address { get; set; }
        public string Desc { get; set; }
        public string Venue { get; set; }
        public string Time { get; set; }
    }
}
