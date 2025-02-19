using Discord;
using Discord.WebSocket;
using Quartz;
using SpicyBot.Entities;
using SpicyBot.Entities.Sumo;

namespace SpicyBot.Sumo.Jobs;

public class StartGameJob(DiscordSocketClient discord, ApplicationDbContext db): IJob
{
    public static readonly JobKey Key = new("start-game", "sumo");
    
    public async Task Execute(IJobExecutionContext context)
    {
        var channel = await discord.GetChannelAsync(1327483353036755037);
        if (channel is not IMessageChannel msgChannel)
        {
            return;
        }

        var poll = await msgChannel.SendMessageAsync(
            "It's almost Sumo time!",
            components: new ComponentBuilder()
                .WithButton("Add additional bet amount", "add-bet-prompt")
                .Build(),
            poll: new PollProperties
            {
                Question = new PollMediaProperties
                {
                    Text = "Pick your fighter!"
                },
                Answers = [
                    new PollMediaProperties
                    {
                        Text = "Blue Guy"
                    },
                ],
                Duration = 2,
                AllowMultiselect = false,
                LayoutType = PollLayout.Default,
            });

        var game = new SumoGame {
            Id = poll.Id
        };
        db.SumoGames.Add(game);
        await db.SaveChangesAsync();
    }
}