using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class CustomerCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        public string CustomerPhoneNumber { get; set; } = string.Empty;
        [Required]
        public string CustomerAddress { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; }
    }
    public class CustomerDisplayDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhoneNumber { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string AddedUserName { get; set; } = string.Empty;
    }
    public class CustomerUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerPhoneNumber { get; set; }
        [Required]
        public string CustomerAddress { get; set; }
    }
}
