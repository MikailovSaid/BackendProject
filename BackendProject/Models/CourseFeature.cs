using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Models
{
    public class CourseFeature
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public string StudentsCount { get; set; }
        public string Assesments { get; set; }
        public string Price { get; set; }
        public int CourseId { get; set; }
        public Course Courses { get; set; }
    }
}
