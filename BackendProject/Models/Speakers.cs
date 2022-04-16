using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Models
{
    public class Speakers
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        [Required]
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
