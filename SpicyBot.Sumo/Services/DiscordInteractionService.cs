using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using SpicyBot.Sumo.Interactions;

namespace SpicyBot.Sumo.Services;

public class DiscordInteractionService(DiscordSocketClient discordClient, IServiceProvider serviceProvider)
    : IHostedService
{
    private readonly InteractionService _interactionService = new(discordClient);

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _interactionService.AddModuleAsync<AddBetInteraction>(serviceProvider);
        discordClient.ButtonExecuted += ButtonExecuted;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        discordClient.ButtonExecuted -= ButtonExecuted;
        await _interactionService.RemoveModuleAsync<AddBetInteraction>();
    }
  
    private async Task ButtonExecuted(SocketMessageComponent interaction)
    {
        var ctx = new SocketInteractionContext<SocketMessageComponent>(discordClient, interaction);
        await _interactionService.ExecuteCommandAsync(ctx, serviceProvider);
    }
}