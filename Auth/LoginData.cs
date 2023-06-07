using System.ComponentModel.DataAnnotations;

namespace GameStore.Client.Auth;

public class LoginData
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }

    public override string ToString()
    {
        return $"{Username} {Password}";
    }
}