using Microsoft.EntityFrameworkCore;

namespace SpicyBot.Entities.Sumo;

[PrimaryKey(nameof(UserId), nameof(SumoGameId))]
public class SumoBet
{
    public int Amount { get; set; }
    
    public ulong UserId { get; init; }
    public User User { get; init; } = null!;
    
    public ulong SumoGameId { get; init; }
    public SumoGame SumoGame { get; init; } = null!;
}