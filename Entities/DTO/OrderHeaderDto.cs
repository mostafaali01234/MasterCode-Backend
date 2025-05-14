using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Entities.DTO
{
    public class OrderHeaderCreateDto
    {
        //public string OrderStatus { get; set; } = SD.StatusNew;
        //public string PaymentStatus { get; set; } = SD.PaymentStatusNew;
        public string OrderNotes { get; set; } = string.Empty;
        //public double OrderTotal { get; set; } = 0;
        [Required]
        public int CustomerId { get; set; }
        public string ApplicationUserId { get; set; }
        //public string? TechId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime InstallDate { get; set; } = DateTime.Now;

    }

    public class OrderCompleteDto
    {
        public int orderId { get; set; }
        public double Paid { get; set; }
        public int MoneySafeId { get; set; }
        public DateTime CompleteDate { get; set; }

    }
    public class OrderHeaderDisplayDto
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; } = SD.StatusNew;
        public string PaymentStatus { get; set; } = SD.PaymentStatusNew;
        public string OrderNotes { get; set; } = string.Empty;
        public double OrderTotal { get; set; } = 0;
        [Required]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ApplicationUserId { get; set; }
        public string ApplicationUserName { get; set; }
        public string? TechId { get; set; }
        public string TechName { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime InstallDate { get; set; } = DateTime.Now;
    }
    public class OrderHeaderUpdateDto
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderNotes { get; set; }
        public double OrderTotal { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public string ApplicationUserId { get; set; }
        public string? TechId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime InstallDate { get; set; }
    }
}
