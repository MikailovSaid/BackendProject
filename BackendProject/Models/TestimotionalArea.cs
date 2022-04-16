using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendProject.Models
{
    public class TestimotionalArea
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Desc { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
