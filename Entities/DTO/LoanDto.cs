using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class LoanCreateDto
    {
        public double Amount { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
        public string EmpId { get; set; }
        public int MoneySafeId { get; set; }
    }
    public class LoanDisplayDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public string Emp { get; set; }
        public string MoneySafe { get; set; }
        public string ApplicationUser { get; set; }
    }
    public class LoanUpdateDto
    {
        public int Id { get; set; }
        public double Amount { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
        public string EmpId { get; set; }
        public int MoneySafeId { get; set; }
    }
}
