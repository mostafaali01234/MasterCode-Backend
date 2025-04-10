using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }
    [DisplayName("اسم العميل")]
    [Required(ErrorMessage = "ادخل اسم العميل")]
    [MaxLength(30)]
    public string CustomerName { get; set; }
    [DisplayName("رقم التليفون")]
    [Required(ErrorMessage = "ادخل رقم التليفون")]
    [Phone]
    public string CustomerPhoneNumber { get; set; }
    [DisplayName("عنوان العميل")]
    [Required(ErrorMessage = "ادخل عنوان العميل")]
    public string CustomerAddress { get; set; }
    [DisplayName("المستخدم")]
    public string? ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    //[ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
}
