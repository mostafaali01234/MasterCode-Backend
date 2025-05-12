using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class Settings
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.Now;
}
