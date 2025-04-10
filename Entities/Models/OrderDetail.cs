using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class OrderDetail
{
    [Key]
    public int Id { get; set; }
    public int OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")]
    //[ValidateNever]
    public OrderHeader OrderHeader { get; set; }
    [Required]
    [DisplayName("اسم الصنف")]
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    //[ValidateNever]
    public Product Product { get; set; }
    [DisplayName("الكمية")]
    public int Count { get; set; }
    [DisplayName("السعر")]
    public double Price { get; set; }
}
