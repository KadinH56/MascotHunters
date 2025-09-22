using UnityEngine;

public static class GameInformation
{
    private static int numPlayers = 1;

    public static int NumPlayers { get => numPlayers; set => numPlayers = value; }
}
