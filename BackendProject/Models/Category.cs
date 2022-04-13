using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
    }
}
