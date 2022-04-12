using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendProject.Models
{
    public class Sliders
    {
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
