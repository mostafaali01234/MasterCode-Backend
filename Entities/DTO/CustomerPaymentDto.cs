using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class CustomerPaymentCreateDto
    {
        public int OrderHeaderId { get; set; }
        public int MoneySafeId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
    public class CustomerPaymentDisplayDto
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public string MoneySafeName { get; set; } 
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string AddedUserName { get; set; }
    }
    public class CustomerPaymentUpdateDto
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public int MoneySafeId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
