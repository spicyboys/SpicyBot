using Microsoft.EntityFrameworkCore;

namespace SpicyBot.Entities.Sumo;

[PrimaryKey(nameof(UserId), nameof(GameId))]
public class SumoBet
{
    public int Amount { get; set; }
    
    public ulong UserId { get; init; }
    public User User { get; init; } = null!;
    
    public int GameId { get; init;}
    public SumoGame SumoGame { get; init; } = null!;
}