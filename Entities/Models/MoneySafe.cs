using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class MoneySafe
{
    public int Id { get; set; }

    public string? Name { get; set; }
    public double? OpeningBalance { get; set; }
    public string? ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    public ApplicationUser ApplicationUser { get; set; }
}
