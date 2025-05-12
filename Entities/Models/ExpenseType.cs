using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class ExpenseType
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

}
