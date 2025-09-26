using UnityEngine;

public static class GameInformation
{
    private static int numPlayers = 1;
    private static int wave = 0;

    private static int totalEnemies = 0;
    private static int enemiesRemaining = 0;

    public static int NumPlayers { get => numPlayers; set => numPlayers = value; }
    public static int Wave { get => wave; set => wave = value; }
    public static int TotalEnemies { get => totalEnemies; set => totalEnemies = value; }
    public static int EnemiesRemaining { get => enemiesRemaining; set => enemiesRemaining = value; }
}
