using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class OrderHeader
{
    [DisplayName("رقم الاوردر")]
    public int Id { get; set; }
    [DisplayName("البائع")]
    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    //[ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
    [DisplayName("تاريخ الاوردر")]
    public DateTime OrderDate { get; set; }
    [DisplayName("تاريخ التسطيب")]
    public DateTime InstallDate { get; set; }
    [DisplayName("الفني")]
    public string? TechId { get; set; }
    [ForeignKey("TechId")]
    //[ValidateNever]
    public ApplicationUser Tech { get; set; }
    [DisplayName("الاجمالي")]
    public double OrderTotal { get; set; }
    [DisplayName("حالة الاوردر")]
    public string? OrderStatus { get; set; }
    [DisplayName("حالة الدفع")]
    public string? PaymentStatus { get; set; }
    [DisplayName("ملاحظات")]
    public string? OrderNotes { get; set; }

    [DisplayName("العميل")]
    [Required(ErrorMessage = "اختر العميل")]
    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    //[ValidateNever]
    public Customer Customer { get; set; }
}
