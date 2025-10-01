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

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// Number of players playing. Will be set in UI
    /// </summary>

    /// <summary>
    /// Radius of which players will spawn in
    /// </summary>
    [SerializeField] private float spawnRadius = 5f;

    /// <summary>
    /// Player's prefab
    /// </summary>
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<Image> playerHealthBars;

    [SerializeField] private bool arcadeBuild = false;

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

            if (arcadeBuild)
            {
                InputDevice[] devices =
                {
                    InputSystem.GetDevice<Keyboard>()
                };
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme(i == 0 ? "ArcadeA" : "ArcadeB", devices);
            }
            else
            {
                string scheme = "MainControlScheme";

                //foreach(InputDevice device in InputSystem.devices)
                //{
                //    print(device);
                //}
                print(InputSystem.GetDevice<Gamepad>());

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
                print(scheme);
                //print(devices[0]);

                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme(scheme, devices);
            }

            player.GetComponent<PlayerMovement>().PlayerID = i;
            player.name = i.ToString();

            //Set player position
            Vector3 spawnPos = Vector3.forward;
            spawnPos = Quaternion.Euler(0, Random.Range(0, 360), 0) * spawnPos;

            spawnPos *= spawnRadius;
            spawnPos += transform.position;
            player.transform.position = spawnPos;

            if(i < playerHealthBars.Count)
            {
                playerHealthBars[i].transform.parent.gameObject.SetActive(true);
                player.GetComponent<PlayerStatManager>().HealthBar = playerHealthBars[i];
            }
            player.GetComponent<PlayerStatManager>().OnAlive();
        }
        //Update, I got it to work
        //Apparently control schemes are a thing, and the bane of my existence
    }
}
