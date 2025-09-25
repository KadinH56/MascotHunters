using UnityEngine;

public static class GameInformation
{
    private static int numPlayers = 1;
    private static int wave = 0;

    public static int NumPlayers { get => numPlayers; set => numPlayers = value; }
    public static int Wave { get => wave; set => wave = value; }
}
