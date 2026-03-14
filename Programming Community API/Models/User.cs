using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Programming_Community_API.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [MaxLength(255)]
        public string password { get; set; }

        [Required]
        [MaxLength(20)]
        public string role { get; set; }
    }
}