using BuildABot.Enums;

namespace BuildABot.Models;
public class BotStateDTO
{
    public int DirectionState { get; set; }
    public required string ElapsedTime { get; set; }
    public int GameTick { get; set; }
    public int PowerUp { get; set; }
    public int SuperPowerUp { get; set; }
    public required Dictionary<Guid, int> LeaderBoard { get; set; }
    public required Location[] BotPostions { get; set; }
    public required PowerUpLocation[] PowerUpLocations { get; set; }
    public required bool[][] Weeds { get; set; }
    public required int[][] HeroWindow { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return $"""
        Elapsed Time: {ElapsedTime}, Game Tick: {GameTick}
        Position: ({X}, {Y}), DirectionState: {(BotAction)DirectionState}
        """;
    }
}
public struct PowerUpLocation
{
    public Location Location { get; set; }
    public int Type { get; set; }
}

public struct Location
{
    public int X { get; set; }
    public int Y { get; set; }
}