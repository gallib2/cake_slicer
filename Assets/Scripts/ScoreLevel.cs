
using System;

public static class ScoreData
{
    public enum ScoreLevel { Awesome = 4, Great = 7, Nice = 10, Regular = 0 };

    public enum ScorePointsByLevel { Awesome = 30, Great = 20, Nice = 10, Regular = 0 };

    public static double NumberOfSlicesScoreNormaliser = 0.5;

    public const int COMBO_MULTIPLIER = 10;
}