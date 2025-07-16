using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class ChatVm
    {
        public ChatVm()
        {
            Rooms = new List<ChatRoom>();
        }
        public int MaxRoomAllowed { get; set; }
        public IList<ChatRoom> Rooms { get; set; }
        public IList<UserVm> Users { get; set; }
        public string? UserId { get; set; }
        public bool AllowAddRoom => Rooms == null || Rooms.Count < MaxRoomAllowed;
    }
}
