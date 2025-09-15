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
    }
}
