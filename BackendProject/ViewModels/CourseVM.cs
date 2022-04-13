using BackendProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewModels
{
    public class CourseVM
    {
        public Course Courses { get; set; }
        public CourseFeature CourseFeatures { get; set; }
        public List<Category> Categories { get; set; }
    }
}
