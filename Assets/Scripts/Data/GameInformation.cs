public static class GameInformation
{
    private static int numPlayers = 1;
    private static int wave = 0;

    private static int totalEnemies = 0;
    private static int enemiesRemaining = 0;

    private static bool isArcadeBuild = false;

    public static int NumPlayers { get => numPlayers; set => numPlayers = value; }
    public static int Wave { get => wave; set => wave = value; }
    public static int TotalEnemies { get => totalEnemies; set => totalEnemies = value; }
    public static int EnemiesRemaining { get => enemiesRemaining; set => enemiesRemaining = value; }
    public static bool IsArcadeBuild { get => isArcadeBuild; set => isArcadeBuild = value; }
}
