using DataAccess.Data;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Structure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatRoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ChatRooms
        [HttpGet("GetChatRoom")]
        //[Route("/[controller]/GetChatRoom")]
        public async Task<ActionResult<IEnumerable<ChatRoom>>> GetChatRoom()
        {
            return await _context.ChatRoom.ToListAsync();
        }


        // GET: api/ChatRooms
        [HttpGet("GetChatUser")]
        //[Route("/[controller]/GetChatUser")]
        public async Task<ActionResult<IEnumerable<object>>> GetChatUser()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var users = await _context.Users.ToListAsync();
            if (users == null)
                return NotFound();

            return users.Where(z => z.Id != userId).Select(z => new { z.Id, z.UserName }).ToList();
        }



        // POST: api/ChatRooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("PostChatRoom")]
        //[Route("/[controller]/PostChatRoom")]
        public async Task<ActionResult<ChatRoom>> PostChatRoom(ChatRoom chatRoom)
        {
            _context.ChatRoom.Add(chatRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatRoom", new { id = chatRoom.Id }, chatRoom);
        }

        // DELETE: api/ChatRooms/5
        [HttpDelete("{id}")]
        //[Route("/[controller]/DeleteChatRoom/{id}")]
        public async Task<IActionResult> DeleteChatRoom(int id)
        {
            var chatRoom = await _context.ChatRoom.FindAsync(id);
            if (chatRoom == null)
            {
                return NotFound();
            }

            _context.PublicChatMessages.RemoveRange(_context.PublicChatMessages.Where(z => z.RoomId == id).ToList());
            _context.ChatRoom.Remove(chatRoom);
            await _context.SaveChangesAsync();

            var room = await _context.ChatRoom.FirstOrDefaultAsync();

            return Ok(new { deleted = id, selected = (room == null ? 0 : room.Id) });
        }

    }
}
