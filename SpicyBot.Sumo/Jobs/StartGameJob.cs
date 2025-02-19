using Discord;
using Discord.WebSocket;
using Quartz;
using SpicyBot.Entities;
using SpicyBot.Entities.Sumo;
using SpicyBot.Sumo.Models;

namespace SpicyBot.Sumo.Jobs;

public class StartGameJob(DiscordSocketClient discord, ApplicationDbContext db) : IJob
{
    public static readonly JobKey Key = new("start-game", "sumo");

    public async Task Execute(IJobExecutionContext context)
    {
        var channel = await discord.GetChannelAsync(1327483353036755037);
        if (channel is not IMessageChannel msgChannel)
        {
            return;
        }

        var components = new ComponentBuilder()
            .WithButton("Add additional bet amount", "add-bet-prompt")
            .Build();

        var poll = new PollProperties
        {
            Question = new PollMediaProperties
            {
                Text = "Pick your fighter!"
            },
            Answers = Enum.GetValues<SumoFighter>()
                .Select(fighter => new PollMediaProperties
                {
                    Text = $"{fighter.GetDisplayName()} Guy",
                    Emoji = fighter.GetEmoji(),
                }).ToList(),
            Duration = 2,
            AllowMultiselect = false,
            LayoutType = PollLayout.Default,
        };

        var message = await msgChannel.SendMessageAsync(
            "It's almost Sumo time!",
            components: components,
            poll: poll);

        var game = new SumoGame
        {
            Id = message.Id
        };
        db.SumoGames.Add(game);
        await db.SaveChangesAsync();
    }
}