using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using SpicyBot.Entities;
using SpicyBot.Services;
using SpicyBot.Sumo;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(app =>
    {
        app.AddUserSecrets<Program>();
    })
    .ConfigureServices((ctx, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                ctx.Configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("SpicyBot.Migrations"));
        });

        services.AddQuartzHostedService();

        services
            .AddSingleton(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.GuildMessagePolls
            })
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(s => new InteractionService(s.GetRequiredService<DiscordSocketClient>()));
            
        services
            .AddHostedService<DiscordLoginService>()
            .AddHostedService<DiscordLoggingService>()
            .AddHostedService<DiscordInteractionService>();
        
        services.AddSumo();
    })
    .Build();

await host.RunAsync();