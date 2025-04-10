using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class ProductCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; } = 0;
        [Required]
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
    public class ProductDisplayDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
    public class ProductUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; } = 0;
        [Required]
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
