using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class Job
{
    public int Id { get; set; }

    [DisplayName("اسم الوظيفة")]
    [Required(ErrorMessage = "ادخل اسم الوظيفة")]
    [MaxLength(30)]
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.Now;
}
