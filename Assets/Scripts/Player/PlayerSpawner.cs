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
    [SerializeField] private SpriteRenderer p2ControlsMove;
    [SerializeField] private SpriteRenderer p2ControlsRoll;
    [SerializeField] private Image p2ControlsUpgrade;

    [SerializeField] private Sprite p2ControlsMoveSprite;
    [SerializeField] private Sprite p2ControlsRollSprite;
    [SerializeField] private Sprite p2ControlsRollUpgrade;

    /// <summary>
    /// Setup the game itself
    /// </summary>
    private void Start()
    {
        p2ControlsMove.enabled = false;
        p2ControlsRoll.enabled = false;
        ////For each player playing, spawn their object
        ////Also sets some internal stuff
        for (int i = 0; i < GameInformation.NumPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab);
            if(i == 1)
            {
                p2ControlsMove.enabled = true;
                p2ControlsRoll.enabled = true;
            }

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
                    p2ControlsMove.sprite = p2ControlsMoveSprite;
                    p2ControlsRoll.sprite = p2ControlsRollSprite;
                    p2ControlsUpgrade.sprite = p2ControlsRollUpgrade;
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
