using BackendProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewModels
{
    public class TeacherVM
    {
        public Teacher Teacher { get; set; }
        public TeacherContact TeacherContact { get; set; }
        public TeacherSkills TeacherSkills { get; set; }
    }
}
