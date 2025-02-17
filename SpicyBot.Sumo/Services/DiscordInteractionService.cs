using Discord.Interactions;
using Microsoft.Extensions.Hosting;
using SpicyBot.Sumo.Interactions;

namespace SpicyBot.Sumo.Services;

public class DiscordInteractionService(
    InteractionService interactionService,
    IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await interactionService.AddModuleAsync<AddBetInteraction>(serviceProvider);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await interactionService.RemoveModuleAsync<AddBetInteraction>();
    }
}