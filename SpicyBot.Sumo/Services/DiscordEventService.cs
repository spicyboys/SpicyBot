using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SpicyBot.Sumo.Services;

public class DiscordEventService(
    DiscordSocketClient client,
    ILogger<DiscordEventService> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        client.PollVoteAdded += PollVoteAdded;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        client.PollVoteAdded -= PollVoteAdded;
        return Task.CompletedTask;
    }

    private Task PollVoteAdded(
        Cacheable<IUser, ulong> user,
        Cacheable<ISocketMessageChannel, IRestMessageChannel, IMessageChannel, ulong> channel,
        Cacheable<IUserMessage, ulong> message,
        Cacheable<SocketGuild, RestGuild, IGuild, ulong>? guild,
        ulong id)
    {
        logger.LogInformation($"Poll vote {id} received by {user.Id} in channel {channel.Id} on message {message.Id}.");
        return Task.CompletedTask;
    }
}