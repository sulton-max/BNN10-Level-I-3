using N75_C.Configurations;
using N75_C.Models.Entities;
using N75_C.Services;

var builder = WebApplication.CreateBuilder(args);
await builder.ConfigureAsync();

var app = builder.Build();

app.MapPost("api/users", async (User user, AccountAggregatorService accountService) => { await accountService.CreateAsync(user); });

await app.ConfigureAsync();
await app.RunAsync();