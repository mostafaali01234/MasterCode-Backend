using System.ComponentModel.DataAnnotations;

namespace Entities.DTO
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword {  get; set; }
        
    }
    public class EditUserDto
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public int JobId { get; set; }
    }
    public class DisplayUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
    }
}
