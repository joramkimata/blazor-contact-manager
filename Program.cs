using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GameStore.Client;
using GameStore.Client.Auth;
using MudBlazor.Services;
using GameStore.Client.GraphQl;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<GraphqlService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();

builder.Services.AddBlazoredToast();

builder.Services.AddScoped<AuthenticationDataMemoryStorage>();
builder.Services.AddScoped<MyAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<MyAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();


await builder.Build().RunAsync();
