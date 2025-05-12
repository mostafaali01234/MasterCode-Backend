using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class ExpenseCreateDto
    {
        public int ExpenseTypeId { get; set; }
        public double Amount { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
        public string EmpId { get; set; }
        public int MoneySafeId { get; set; }
    }
    public class ExpenseDisplayDto
    {
        public int Id { get; set; }
        public string ExpenseType { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
        public string Emp { get; set; } = string.Empty;
        public string MoneySafe { get; set; } = string.Empty;
        public string ApplicationUser { get; set; } = string.Empty;
    }
    public class ExpenseUpdateDto
    {
        public int Id { get; set; }
        public int ExpenseTypeId { get; set; }
        public double Amount { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
        public string EmpId { get; set; }
        public int MoneySafeId { get; set; }
    }
}
