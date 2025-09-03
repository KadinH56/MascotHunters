/*****************************************************************************
// File Name : PlayerSpawner
// Author : Lucas Fehlberg
// Creation Date : September 2, 2025
// Last Updated : September 2, 2025
//
// Brief Description : Actually spawns players into the game based on the number of players playing
*****************************************************************************/

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// Number of players playing. Will be set in UI
    /// </summary>
    [SerializeField] private int numPlayers;

    /// <summary>
    /// Radius of which players will spawn in
    /// </summary>
    [SerializeField] private float spawnRadius = 5f;

    /// <summary>
    /// Player's prefab
    /// </summary>
    [SerializeField] private GameObject playerPrefab;

    private PlayerInputManager pInputManager;

    /// <summary>
    /// Setup the game itself
    /// </summary>
    private void Start()
    {
        pInputManager = GetComponent<PlayerInputManager>();

        ////For each player playing, spawn their object
        ////Also sets some internal stuff
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab);

            player.GetComponent<PlayerMovement>().PlayerID = i;
            player.name = i.ToString();

            //Set player position
            Vector3 spawnPos = Vector3.forward;
            spawnPos = Quaternion.Euler(0, Random.Range(0, 360), 0) * spawnPos;

            spawnPos *= spawnRadius;
            spawnPos += transform.position;
            player.transform.position = spawnPos;
        }
        //Update, I got it to work
        //Apparently control schemes are a thing, and the bane of my existence

        //Why is there absolutely 0 documentation on manually joining players in unity this is actually insane
        //Every time I **THINK** I find something, I get told "no" and its a freaking thread that gets bumped 7 times
        //But nobody tries to help
        //I have one lead and it's (note, I was never able to find this thread)

        //TODO: Realize that unity is stupid & so am I for thinking this would be simple & ORDERING MY LIST OF TODOS
        //FROM BOTTOM TO TOP

        //TODO: Realize I'm an idiot and need to try again

        //TODO: Fix the NullRef from spawning in the second player. I do not know why this is happening, this
        //Sounds like an issue I talk to Zach for if I can't figure it out beforehand
    }

    /// <summary>
    /// Handles player joining. Seems unused but is actually integral to making the game work
    /// Don't ask blame unity for weird workings
    /// </summary>
    /// <param name="playerInput"></param>
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        foreach(var device in playerInput.devices)
        {
            print(device.name);
            break;
        }
    }
}
