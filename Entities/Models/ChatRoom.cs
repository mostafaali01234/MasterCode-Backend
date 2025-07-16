using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class ChatRoom
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}
