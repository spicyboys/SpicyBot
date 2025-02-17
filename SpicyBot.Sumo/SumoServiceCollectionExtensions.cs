using Microsoft.Extensions.DependencyInjection;
using SpicyBot.Sumo.Services;

namespace SpicyBot.Sumo;

public static class SumoServiceCollectionExtensions
{
    public static IServiceCollection AddSumo(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddHostedService<DiscordEventService>()
            .AddHostedService<DiscordInteractionService>();
    }
}