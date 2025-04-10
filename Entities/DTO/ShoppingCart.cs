using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Entities.DTO
{
    public class ShoppingCartCreateDto
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int Count { get; set; } = 1;
        public double Price { get; set; } = 0;
    }
    public class ShoppingCartDisplayDto
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
    public class ShoppingCartUpdateDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
