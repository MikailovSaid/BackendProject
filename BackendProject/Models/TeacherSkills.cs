using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Models
{
    public class TeacherSkills
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public string TeamLeader { get; set; }
        public string Development { get; set; }
        public string Design { get; set; }
        public string Innovation { get; set; }
        public string Communication { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
