using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class CommissionDto
    {
        public int OrderId { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public double OrderTotal { get; set; }
        public double Comm { get; set; }
        public DateTime InstallDate { get; set; }
    }
}
