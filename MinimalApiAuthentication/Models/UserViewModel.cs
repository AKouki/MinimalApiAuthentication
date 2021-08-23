
using System.ComponentModel.DataAnnotations;

namespace MinimalApiAuthentication.Models;
public class UserViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
