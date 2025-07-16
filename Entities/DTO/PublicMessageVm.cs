using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class PublicMessageVm
    {
        public string Message { get; set; }
        public string SenderName { get; set; }
        public string SenderId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public DateTime Time { get; set; }
    }
}
