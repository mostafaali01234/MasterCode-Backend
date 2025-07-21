using DataAccess.Data;
using Entities.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Structure.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;
        public ChatHub(ApplicationDbContext db)
        {
            _db = db;
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiverUserConnected", userId, userName);

                HubConnections.AddUserConnection(userId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (HubConnections.HasUserConnection(userId, Context.ConnectionId))
            {
                var UserConnections = HubConnections.Users[userId];
                UserConnections.Remove(Context.ConnectionId);
                HubConnections.Users.Remove(userId);

                if (UserConnections.Any())
                    HubConnections.Users.Add(userId, UserConnections);
            }


            if (!string.IsNullOrEmpty(userId))
            {
                var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiverUserDisconnected", userId, userName);

                HubConnections.AddUserConnection(userId, Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }

        // New -------------------------------------

        public async Task SendAddRoom(int maxRoom, string roomName)
        {
            var userId1 = ClaimsPrincipal.Current?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;
            var newRoom = new ChatRoom
            {
                Name = roomName,
            };

            _db.ChatRoom.Add(newRoom);
            await _db.SaveChangesAsync();

            var roomsList =  await _db.ChatRoom.ToListAsync();

            await Clients.All.SendAsync("ReceiveAddRoomMessage", maxRoom, newRoom.Id, roomName, userId, userName, roomsList);
        }
        public async Task SendDelRoom(int roomId, string roomName)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;

            var chatRoom = await _db.ChatRoom.FindAsync(roomId);
            _db.PublicChatMessages.RemoveRange(_db.PublicChatMessages.Where(z => z.RoomId == roomId).ToList());
            _db.ChatRoom.Remove(chatRoom);
            await _db.SaveChangesAsync();

            var roomsList = await _db.ChatRoom.ToListAsync();

            //await Clients.All.SendAsync("ReceiveDelRoomMessage", roomId, roomName, userName);

            await Clients.All.SendAsync("ReceiveAddRoomMessage", 0, roomId, roomName, userId, userName, roomsList);
        }
        public async Task populateRoomMessages(int roomId)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = userId == null ? "" : _db.Users.FirstOrDefault(u => u.Id == userId).UserName;

            var messagesList = _db.PublicChatMessages
                 .Where(z => z.RoomId == roomId)
                 .OrderBy(z => z.Time)
                 .Select(z => new PublicMessageVm
                 {
                     RoomId = (int)z.RoomId,
                     RoomName = z.ChatRoom.Name,
                     Message = z.Message,
                     SenderId = z.SenderId,
                     SenderName = z.Sender.UserName,
                     Time = z.Time
                 })
                 .ToList();

            await Clients.All.SendAsync("populateRoomMessages", roomId, userId, messagesList);
        }
        public async Task SendPublicMessage(int roomId, string message, string roomName)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;
            var newMessage = new Entities.Models.PublicChatMessages
            {
                RoomId = roomId,
                SenderId = userId,
                Message = message,
                Time = DateTime.Now
            };
            _db.PublicChatMessages.Add(newMessage);
            _db.SaveChanges();

            var messagesList = _db.PublicChatMessages
               .Where(z => z.RoomId == roomId)
               .OrderBy(z => z.Time)
               .Select(z => new PublicMessageVm
               {
                   RoomId = (int)z.RoomId,
                   RoomName = z.ChatRoom.Name,
                   Message = z.Message,
                   SenderId = z.SenderId,
                   SenderName = z.Sender.UserName,
                   Time = z.Time
               })
               .ToList();

            //await Clients.All.SendAsync("ReceivePublicMessage", roomId, userId, userName, newMessage, roomName);
            await Clients.All.SendAsync("populateRoomMessages", roomId, userId, messagesList);
        }



        public async Task populatePrivateChat(string senderId, string receiverId)
        {
            var messagesList = _db.PrivateChatMessages
                 .Where(z => (z.SenderId == senderId && z.ReceiverId == receiverId) || (z.SenderId == receiverId && z.ReceiverId == senderId))
                 .OrderBy(z => z.Time)
                 .Select(z => new PrivateMessageVm
                 {
                     Message = z.Message,
                     SenderId = z.SenderId,
                     SenderName = z.Sender.UserName,
                     ReceiverId = z.ReceiverId,
                     ReceiverName = z.Receiver.UserName,
                     Time = z.Time,
                     Seen = z.Seen
                 })
                 .ToList();

            await Clients.All.SendAsync("populatePrivateChat", senderId, receiverId, messagesList);
        }

        public async Task SendPrivateMessage(string receiverId, string message, string receiverName)
        {
            var senderId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var senderName = _db.Users.FirstOrDefault(u => u.Id == senderId).UserName;

            var newMessage = new Entities.Models.PrivateChatMessages
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                Seen = false,
                Time = DateTime.Now
            };
            _db.PrivateChatMessages.Add(newMessage);
            _db.SaveChanges();


            await populatePrivateChat(senderId, receiverId);
        }

        // Old -------------------------------------

        public async Task SendAddRoomMessage(int maxRoom, int roomId, string roomName)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;
            var newRoom = new ChatRoom
            {
                Name = roomName,
            };

            _db.ChatRoom.Add(newRoom);
            await _db.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveAddRoomMessage", maxRoom, roomId, roomName, userId, userName);
        }
        public async Task SendDelRoomMessage(int deleted, int selected, string roomName)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;

            await Clients.All.SendAsync("ReceiveDelRoomMessage", deleted, selected, roomName, userName);
        }

        public async Task SendOpenPrivateChat(string receiverId)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == userId).UserName;

            await Clients.User(receiverId).SendAsync("ReceiveOpenPrivateChat", userId, userName);
        }
        public async Task SendDeletePrivateChat(string chatId)
        {
            await Clients.All.SendAsync("ReceiveDeletePrivateChat", chatId);
        }
    }
}
