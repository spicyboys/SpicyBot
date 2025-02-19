using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using SpicyBot.Sumo.Jobs;

namespace SpicyBot.Sumo.Services;

public class DiscordEventService(
    DiscordSocketClient client,
    ILogger<DiscordEventService> logger,
    ISchedulerFactory schedulerFactory) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        client.Ready += Ready;
        client.MessageUpdated += MessageUpdated;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        client.Ready -= Ready;
        client.MessageUpdated -= MessageUpdated;
        return Task.CompletedTask;
    }

    private async Task Ready()
    {
        var scheduler = await schedulerFactory.GetScheduler();
        await scheduler.TriggerJob(StartGameJob.Key);
    }

    private Task MessageUpdated(
        Cacheable<IMessage, ulong> message,
        SocketMessage socketMessage,
        ISocketMessageChannel channel)
    {
        logger.LogInformation($"Message {message.Id} updated by {socketMessage.Id}.");
        return Task.CompletedTask;
    }
}