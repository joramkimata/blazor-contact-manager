using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using GameStore.Client.Utils;
using Microsoft.AspNetCore.Components.Authorization;

namespace GameStore.Client.Auth;

public class MyAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationDataMemoryStorage _authenticationDataMemoryStorage;
    private readonly ILocalStorageService _localStorageService;

    public string Username { get; set; } = "";

    public MyAuthenticationStateProvider(HttpClient httpClient,
        AuthenticationDataMemoryStorage authenticationDataMemoryStorage, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _authenticationDataMemoryStorage = authenticationDataMemoryStorage;
        _localStorageService = localStorageService;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var identity = new ClaimsIdentity();
        var token = await _localStorageService.GetItemAsync<String>(LocalStorageConstants.TokenKey);
        var permissions = await _localStorageService.GetItemAsync<List<String>>(LocalStorageConstants.Permissions);

        if (tokenHandler.CanReadToken(token))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            identity = new ClaimsIdentity(jwtSecurityToken.Claims, "Blazor");
        }

        if (permissions != null)
            foreach (var role in permissions)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

        var principal = new ClaimsPrincipal(identity);
        var authenticationState = new AuthenticationState(principal);
        var authenticationTask = Task.FromResult(authenticationState);

        return await authenticationTask;
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

        if (tokenResponse is not { access_token: not null }) return false;

        //_authenticationDataMemoryStorage.Token = tokenResponse.access_token;
        //_authenticationDataMemoryStorage.Permissions = tokenResponse.permissions;

        if (!await _localStorageService.ContainKeyAsync(LocalStorageConstants.TokenKey))
        {
            await _localStorageService.SetItemAsync(LocalStorageConstants.TokenKey, tokenResponse.access_token);
        }

        if (!await _localStorageService.ContainKeyAsync(LocalStorageConstants.Permissions))
        {
            await _localStorageService.SetItemAsync(LocalStorageConstants.Permissions, tokenResponse.permissions);
        }

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return true;
    }

    public async void Logout()
    {
        //_authenticationDataMemoryStorage.Token = "";
        await _localStorageService.ClearAsync();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        var authenticationState = await task;

        {
            var userClaims = authenticationState.User.Claims;
            var roleClaimType = ClaimTypes.Role;
            var roles = userClaims
                .Where(c => c.Type == roleClaimType)
                .Select(c => c.Value)
                .ToList();
        }
    }

    public void Dispose()
    {
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
    }
}