using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class ShoppingCart
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    //[ValidateNever]
    public Product Product { get; set; }
    [Range(1, 10000, ErrorMessage = "ادخل رقم صحيح اكبر من 0")]
    public int Count { get; set; }
    [Range(1, 10000, ErrorMessage = "ادخل سعر صحيح اكبر من 0")]
    public double Price { get; set; }
    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    //[ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
}
