using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class UserBalacneDto
    {
        public int OperationId { get; set; }
        public string OperationType { get; set; } = string.Empty;
        public DateTime OperationDate { get; set; }
        public string MoneysafeName { get; set; } = string.Empty;
        public string OperationNotes { get; set; } = string.Empty;
        public double OperationTotal { get; set; } 
        public double Balance { get; set; }
    }
}
