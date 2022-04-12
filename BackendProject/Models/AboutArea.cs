using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendProject.Models
{
    public class AboutArea
    {
        public int Id { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Image { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
