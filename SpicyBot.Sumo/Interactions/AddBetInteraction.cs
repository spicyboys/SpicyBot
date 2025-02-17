using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using SpicyBot.Entities;
using SpicyBot.Entities.Sumo;

namespace SpicyBot.Sumo.Interactions;

public class AddBetInteraction(ApplicationDbContext db)
    : InteractionModuleBase<SocketInteractionContext<SocketMessageComponent>>
{
    [ComponentInteraction("add-bet-prompt:*")]
    public async Task AddBetPrompt(string gameId)
    {
        var user = await db.Users.FindAsync(Context.User.Id);
        if (user == null || user.PointBalance == 0)
        {
            await Context.Interaction.RespondAsync(
                "You can't change your bet amount because you don't have any more points.",
                ephemeral: true);
            return;
        }

        var bet = await db.SumoBets
            .Where(s => s.UserId == user.Id && s.GameId == int.Parse(gameId))
            .SingleOrDefaultAsync();

        await Context.Interaction.RespondAsync(
            $"Your current bet is {bet?.Amount ?? 0} and your point balance is {user.PointBalance}",
            components: BuildBetAmountButtons(user, gameId));
    }

    [ComponentInteraction("bet-amount:*:*")]
    public async Task AddBetAmount(string gameId, string amount)
    {
        var bet = await db.SumoBets
            .Where(s => s.UserId == Context.User.Id && s.GameId == int.Parse(gameId))
            .SingleOrDefaultAsync();
        if (bet == null)
        {
            bet = new SumoBet()
            {
                UserId = Context.User.Id,
                GameId = int.Parse(gameId),
                Amount = int.Parse(amount)
            };
            db.SumoBets.Add(bet);
        }
        else
        {
            bet.Amount = int.Parse(amount);
        }
        
        await db.SaveChangesAsync();

        await Context.Interaction.UpdateAsync(m => m.Content =$"Bet updated to {bet.Amount}");
    }

    private MessageComponent BuildBetAmountButtons(User user, string gameId)
    {
        var builder = new ComponentBuilder();
        if (user.PointBalance < 3)
        {
            for (var i = 1; i <= user.PointBalance; i++)
            {
                builder.WithButton(i.ToString(), $"bet-amount:{gameId}:{i}");
            }
        }
        else
        {
            builder
                .WithButton("1", $"bet-amount:1:{gameId}")
                .WithButton((user.PointBalance / 2).ToString(), $"bet-amount:{gameId}:{user.PointBalance / 2}")
                .WithButton(user.PointBalance.ToString(), $"bet-amount:{gameId}:{user.PointBalance}");
        }

        return builder.Build();
    }
}