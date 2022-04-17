using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewModels.Admin
{
    public class TeacherVM
    {
        public string Name { get; set; }
        public string Profession { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public IFormFile Photo { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string Language { get; set; }
        public string TeamLeader { get; set; }
        public string Development { get; set; }
        public string Design { get; set; }
        public string Innovation { get; set; }
        public string Communication { get; set; }

    }
}
