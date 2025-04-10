using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class Category
{
    public int Id { get; set; }

    [DisplayName("اسم التصنيف")]
    [Required(ErrorMessage = "ادخل اسم للتصنيف")]
    [MaxLength(30)]
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.Now;

    [DisplayName("الترتيب فالعرض")]
    [Required(ErrorMessage = "ادخل رقم للترتيب فالقائمة")]
    [Range(1, 100, ErrorMessage = "رقم بين 1 و 100")]
    public int DisplayOrder { get; set; }
}
