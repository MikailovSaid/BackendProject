using BackendProject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewModels.Admin
{
    public class CourseVM
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string AboutCourse { get; set; }
        public string AboutDesc { get; set; }
        public string ApplyCourse { get; set; }
        public string ApplyDesc { get; set; }
        public string CertificationCourse { get; set; }
        public string CertificationDesc { get; set; }
        public IFormFile Photo { get; set; }
        public int CategoryId { get; set; }

        public string StartDate { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public string StudentsCount { get; set; }
        public string Assesments { get; set; }
        public string Price { get; set; }
    }
}
