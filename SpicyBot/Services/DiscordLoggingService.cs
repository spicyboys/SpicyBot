using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace SpicyBot.Services;

public class DiscordLoggingService(DiscordSocketClient discord, InteractionService interactionService): IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        discord.Log += LogAsync;
        interactionService.Log += LogAsync;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        discord.Log -= LogAsync;
        interactionService.Log -= LogAsync;
        return Task.CompletedTask;
    }
    
    private Task LogAsync(LogMessage message)
    {
        if (message.Exception is CommandException cmdException)
        {
            Console.WriteLine($"[Command/{message.Severity}] {cmdException.Command.Aliases.First()}"
                              + $" failed to execute in {cmdException.Context.Channel}.");
            Console.WriteLine(cmdException);
        }
        else 
            Console.WriteLine($"[General/{message.Severity}] {message}");

        return Task.CompletedTask;
    }
}