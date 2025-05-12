using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class CustomerPayment
{
    [Key]
    public int Id { get; set; }
    //public int CustomerId { get; set; }
    //[ForeignKey("CustomerId")]
    //public Customer Customer { get; set; }
    public int? OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")]
    public OrderHeader? OrderHeader { get; set; }
    public double? Amount { get; set; }
    public DateTime? Date { get; set; }
    public int? MoneySafeId { get; set; }
    [ForeignKey("MoneySafeId")]
    public MoneySafe MoneySafe { get; set; }
    public string? ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    //[ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
}
