using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class LoginViewModel : IdentityUser
{
    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
