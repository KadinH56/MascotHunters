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
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// Number of players playing. Will be set in UI
    /// </summary>

    /// <summary>
    /// Radius of which players will spawn in
    /// </summary>
    [SerializeField] private float spawnRadius = 2f;

    /// <summary>
    /// Player's prefab
    /// </summary>
    [SerializeField] private GameObject playerPrefab;

    /// <summary>
    /// Setup the game itself
    /// </summary>
    private void Start()
    {
        ////For each player playing, spawn their object
        ////Also sets some internal stuff
        for (int i = 0; i < GameInformation.NumPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab);

            //You know, at least this works on arcade builds
            //Unity I freaking hate your input system with a passion idk how you do this

            if (GameInformation.IsArcadeBuild)
            {
                InputDevice[] devices =
                {
                    InputSystem.GetDevice<Keyboard>()
                };
                player.GetComponent<PlayerStatManager>().SetControls(i == 0 ? "ArcadeA" : "ArcadeB", devices);
            }
            else
            {
                string scheme = "MainControlScheme";

                InputDevice[] devices = new InputDevice[1];
                if (InputSystem.devices.Count > 1)
                {
                    devices[0] = InputSystem.devices[i];
                    //print(devices[0]);

                    //if (devices[0].name == "Gamepad")
                    //{
                    //    scheme = "Gamepad";
                    //}
                    //print(scheme);
                } 
                else
                {
                    devices[0] = InputSystem.GetDevice<Keyboard>();
                    if(i == 1)
                    {
                        scheme = "Keyboard2";
                    }
                }
                //print(devices[0]);
                player.GetComponent<PlayerStatManager>().SetControls(scheme, devices);//.SwitchCurrentControlScheme(scheme, devices);
            }

            player.GetComponent<PlayerMovement>().PlayerID = i;
            player.GetComponent<PlayerMovement>().SetSprite();
            player.name = i.ToString();

            //Set player position
            Vector3 spawnPos = Vector3.forward;
            spawnPos = Quaternion.Euler(0, Random.Range(0, 10), 0) * spawnPos;  //changed this line - Drew

            spawnPos *= spawnRadius;
            spawnPos += transform.position;
            player.transform.position = spawnPos;

            player.GetComponent<PlayerStatManager>().OnAlive();

            //player.GetComponent<UpgradeSystem>().StartUpgrades(true);
        }
        //Update, I got it to work
        //Apparently control schemes are a thing, and the bane of my existence
    }
}
