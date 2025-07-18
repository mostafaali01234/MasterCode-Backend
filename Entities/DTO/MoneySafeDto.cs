﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class MoneySafeCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        public double OpeningBalance { get; set; } = 0;
        public string ApplicationUserId { get; set; }
    }
    public class MoneySafeDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double OpeningBalance { get; set; } = 0;
        public double CurrentBalance { get; set; } = 0;
        public string ApplicationUser { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
    }
    public class MoneySafeMovesDisplayDto
    {
        public int opId { get; set; }
        public DateTime? opDate { get; set; }
        public string opType { get; set; }
        public string? opNotes { get; set; }
        public double? opTotal { get; set; }
        public double balance { get; set; } 
    }
    public class MoneySafeUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        public double OpeningBalance { get; set; } = 0;
        public string ApplicationUserId { get; set; }
    }
}
