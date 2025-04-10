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
    public class OrderCreateDto
    {
        public OrderHeaderCreateDto OrderHeader { get; set; }
        public IEnumerable<OrderDetailCreateDto> OrderDetailsList { get; set; }
    }
    public class OrderDisplayDto
    {
        public OrderHeaderDisplayDto OrderHeader { get; set; }
        public IEnumerable<OrderDetailDisplayDto> OrderDetailsList { get; set; }
    }
    public class OrderUpdateDto
    {
        public OrderHeaderUpdateDto OrderHeader { get; set; }
        public IEnumerable<OrderDetailUpdateDto> OrderDetailsList { get; set; }
    }
}
