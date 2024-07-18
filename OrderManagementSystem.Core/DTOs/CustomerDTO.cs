using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Core.DTOs
{
    public class CustomerDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
