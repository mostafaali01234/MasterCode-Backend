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
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        //[Required]
        //public string Password { get; set; }
        //public string Oldpassword { get; set; }
        //[Compare("Password")]
        //public string ConfirmPassword { get; set; }
        public int JobId { get; set; }
        public string RoleId { get; set; }
    }public class EditUserPasswordDto
    {
        public string Id { get; set; }
        public string Oldpassword { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
    public class DisplayUserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
