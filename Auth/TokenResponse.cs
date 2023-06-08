

namespace GameStore.Client.Auth;

public class TokenResponse
{
    public string? access_token { get; set; }
    public string? refresh_token { get; set; }
    public List<string>? permissions { get; set; }
}