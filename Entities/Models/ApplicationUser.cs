using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DisplayName("المستخدم")]
        public string Name { get; set; }
        [NotMapped]
        public string Role { get; set; }
        public int JobId { get; set; }

        [ForeignKey("JobId")]
        //[ValidateNever]
        [DisplayName("الوظيفة")]
        public Job Job { get; set; }
    }
}
