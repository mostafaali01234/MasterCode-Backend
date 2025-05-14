using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Loan
{
    public int Id { get; set; }
    public double? Amount { get; set; }
    public DateTime? Date { get; set; }
    public string Notes { get; set; }
    public string EmpId { get; set; }
    [ForeignKey("EmpId")]
    public ApplicationUser Emp { get; set; }
    public int? MoneySafeId { get; set; }
    [ForeignKey("MoneySafeId")]
    public MoneySafe MoneySafe { get; set; }
    public string? ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    public ApplicationUser ApplicationUser { get; set; }
}
