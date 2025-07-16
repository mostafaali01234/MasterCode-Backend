using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class PublicChatMessages
{
    public int Id { get; set; }
    [Required]
    public string Message { get; set; }
    public string? SenderId { get; set; }
    [ForeignKey("SenderId")]
    public ApplicationUser? Sender { get; set; }
    public int? RoomId { get; set; }
    [ForeignKey("RoomId")]
    public ChatRoom? ChatRoom { get; set; }
    public DateTime Time { get; set; }
}
