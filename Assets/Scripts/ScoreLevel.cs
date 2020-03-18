

public static class ScoreData
{
    //Settings before 14/03/2020:
    //public enum ScoreLevel { Awesome = 2, Great = 5, Nice = 8, Regular = 0 };
    //Settings for testing:
    public enum ScoreLevel { Awesome = 7, Great = 9, Nice = 12, Regular = 0 };
    //public enum ScoreLevel { Awesome = 4, Great = 7, Nice = 10, Regular = 0 };

    public enum ScorePointsByLevel { Awesome = 30, Great = 20, Nice = 10, Regular = 0 };

    public static double NumberOfSlicesScoreNormaliser = 0.5;

    public const int COMBO_MULTIPLIER = 10;
}