using System.ComponentModel.DataAnnotations;

namespace BackendProject.ViewModels.Admin
{
    public class ServiceAreaVM
    {
        public int Id { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
