using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace GameStore.Client.Auth;

public class MyAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationDataMemoryStorage _authenticationDataMemoryStorage;

    public string Username { get; set; } = "";

    public MyAuthenticationStateProvider(HttpClient httpClient,
        AuthenticationDataMemoryStorage authenticationDataMemoryStorage)
    {
        _httpClient = httpClient;
        _authenticationDataMemoryStorage = authenticationDataMemoryStorage;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var identity = new ClaimsIdentity();

        if (tokenHandler.CanReadToken(_authenticationDataMemoryStorage.Token))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(_authenticationDataMemoryStorage.Token);
            identity = new ClaimsIdentity(jwtSecurityToken.Claims, "Blazor");
        }

        var principal = new ClaimsPrincipal(identity);
        var authenticationState = new AuthenticationState(principal);
        var authenticationTask = Task.FromResult(authenticationState);

        return authenticationTask;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var data = new
        {
            username,
            password
        };
        var response = await _httpClient.PostAsJsonAsync("http://localhost:8030/auth/login", data);

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

        if (tokenResponse is not { token: not null }) return false;
        _authenticationDataMemoryStorage.Token = tokenResponse.token;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return true;
    }

    public void Logout()
    {
        _authenticationDataMemoryStorage.Token = "";
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        var authenticationState = await task;

        if (authenticationState is not null)
        {
            var userClaims = authenticationState.User.Claims;
            var userIdClaim = userClaims.FirstOrDefault(c => c.Type == "userId");
            var iatClaim = userClaims.FirstOrDefault(c => c.Type == "iat");
            var expClaim = userClaims.FirstOrDefault(c => c.Type == "exp");

            int userId = 0;
            int iat = 0;
            int exp = 0;

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            if (iatClaim != null && int.TryParse(iatClaim.Value, out int parsedIat))
            {
                iat = parsedIat;
            }

            if (expClaim != null && int.TryParse(expClaim.Value, out int parsedExp))
            {
                exp = parsedExp;
            }

            // Use the extracted values as needed
            Console.WriteLine($"userId: {userId}");
            Console.WriteLine($"iat: {iat}");
            Console.WriteLine($"exp: {exp}");
        }
    }

    public void Dispose()
    {
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
    }
}