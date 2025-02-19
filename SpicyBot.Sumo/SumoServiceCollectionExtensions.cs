using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SpicyBot.Sumo.Jobs;
using SpicyBot.Sumo.Services;

namespace SpicyBot.Sumo;

public static class SumoServiceCollectionExtensions
{
    public static IServiceCollection AddSumo(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddHostedService<DiscordEventService>()
            .AddHostedService<DiscordInteractionService>()
            .AddQuartz(q =>
            {
                q.AddJob<StartGameJob>(o => o.WithIdentity(StartGameJob.Key).StoreDurably());
            });
    }
}