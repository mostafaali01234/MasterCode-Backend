using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class PrivateMessageVm
    {
        public string Message { get; set; }
        public string SenderName { get; set; }
        public string SenderId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverId { get; set; }
        public DateTime Time { get; set; }
        public bool Seen { get; set; }
    }
}
