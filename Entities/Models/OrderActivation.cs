using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class OrderActivation
{
    public int Id { get; set; }
    public int? OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")]
    public OrderHeader? OrderHeader { get; set; }
    public string? DeviceCode { get; set; }
    public string? ActivationCode { get; set; }
}
