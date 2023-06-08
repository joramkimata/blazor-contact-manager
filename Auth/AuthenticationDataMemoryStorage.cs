namespace GameStore.Client.Auth;

public class AuthenticationDataMemoryStorage
{
    public string Token { get; set; } = "";

    public List<string>? Permissions { get; set; } = default;
}