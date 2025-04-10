using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Product
{
    [Key]
    public int Id { get; set; }


    [DisplayName("اسم الصنف")]
    [Required(ErrorMessage = "ادخل اسم للصنف")]
    [MaxLength(30)]
    public string Title { get; set; }


    [Required(ErrorMessage = "ادخل وصف للصنف")]
    [DisplayName("وصف الصنف")]
    public string Description { get; set; }


    [Required(ErrorMessage = "ادخل سعر للصنف")]
    [DisplayName("سعر الصنف")]
    [Range(1, 50000, ErrorMessage = "ادخل سعر صحيح")]
    public decimal Price { get; set; }


    [Required(ErrorMessage = "اختر تصنيف")]
    public int CategoryId { get; set; }


    [ForeignKey("CategoryId")]
    //[ValidateNever]
    [DisplayName("التصنيف")]
    [Required(ErrorMessage = "اختر تصنيف")]
    public Category Category { get; set; }


    //[ValidateNever]
    [DisplayName("صورة الصنف")]
    public string? ImageUrl { get; set; }
}
