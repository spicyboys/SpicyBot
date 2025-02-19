using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace SpicyBot.Services;

public class DiscordInteractionService(
    DiscordSocketClient client,
    InteractionService interactionService,
    IServiceProvider serviceProvider) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        client.InteractionCreated += InteractionCreated;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        client.InteractionCreated -= InteractionCreated;
        return Task.CompletedTask;
    }

    private async Task InteractionCreated(SocketInteraction interaction)
    {
        IInteractionContext ctx;
        if (interaction is SocketMessageComponent i)
        {
            ctx = new SocketInteractionContext<SocketMessageComponent>(client, i);
        }
        else
        {
            ctx = new SocketInteractionContext(client, interaction);
        }

        await interactionService.ExecuteCommandAsync(ctx, serviceProvider);
    }
}