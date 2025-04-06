namespace SlotGame.Domain.Constants;

public static class BetConstants
{
    public const int MinBet = 1;
    public const int MaxBet = 10;

    public const int TotalPercent = 100;
    public const int WinChancePercent = 40;
    public const int BigWinChancePercent = 10;
    public static int LossChancePercent => TotalPercent - WinChancePercent - BigWinChancePercent;

    public const decimal BaseWinMinMultiplier = 0.1m; // chosen as the min multiplier for a win
    public const decimal BaseWinMaxMultiplier = 2.0m;

    public const decimal BigWinMinMultiplier = 2;
    public const decimal BigWinMaxMultiplier = 10;
}
