using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SpicyBot.Services;

public class DiscordLoginService(
    DiscordSocketClient discord,
    IConfiguration config) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await discord.LoginAsync(TokenType.Bot, config["Discord:Token"]);
        await discord.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await discord.LogoutAsync();
        await discord.StopAsync();
    }
}