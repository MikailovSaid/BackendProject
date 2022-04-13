using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }
        [Required]
        public string AboutCourse { get; set; }
        [Required]
        public string AboutDesc { get; set; }
        public string ApplyCourse { get; set; }
        public string ApplyDesc { get; set; }
        public string CertificationCourse { get; set; }
        public string CertificationDesc { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
        public List<CourseFeature> CourseFeatures { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
