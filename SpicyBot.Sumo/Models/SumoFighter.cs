using Discord;

namespace SpicyBot.Sumo.Models;

public enum SumoFighter
{
    Blue,
    Green,
    Grey,
    Brown
}

internal static class SumoFighterExtensions
{
    public static string GetDisplayName(this SumoFighter fighter) => fighter switch
    {
        SumoFighter.Blue => "Blue",
        SumoFighter.Green => "Green",
        SumoFighter.Grey => "Grey",
        SumoFighter.Brown => "Brown",
        _ => throw new ArgumentOutOfRangeException(nameof(fighter), fighter, null)
    };

    public static IEmote GetEmoji(this SumoFighter fighter) => fighter switch
    {
        SumoFighter.Blue => new Emoji("\uD83D\uDFE6"),
        SumoFighter.Green => new Emoji("\uD83D\uDFE9"),
        SumoFighter.Grey => new Emoji("\u2B1B"),
        SumoFighter.Brown => new Emoji("\uD83D\uDFEB"),
        _ => throw new ArgumentOutOfRangeException(nameof(fighter), fighter, null)
    };
}