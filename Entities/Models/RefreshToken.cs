using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
    public DateTime Expires { get; set; }
    public ApplicationUser? User { get; set; }
}
