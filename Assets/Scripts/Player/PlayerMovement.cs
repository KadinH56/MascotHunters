/*****************************************************************************
// File Name : PlayerMovement
// Author : Lucas Fehlberg
// Creation Date : September 2, 2025
// Last Updated : September 2, 2025
//
// Brief Description : Player movement. Also turns on the action map and handles mapping to player movement and some
//  Special abilities, including dodge roll
*****************************************************************************/

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody pRigidBody;

    //Used to ID the player for local multiplayer. Set to 0 or 1 ingame
    private int playerID = -1;

    /// <summary>
    /// Used by the Player Manager to set the player ID to 0 or 1
    /// </summary>
    public int PlayerID { get => playerID; set => playerID = value; }

    /// <summary>
    /// Sets some private variables and starts the action map
    /// Handled in PMovement because this is garunteed to be active upon loading the scene
    /// </summary>
    public void Awake()
    {
        //Init above variables
        pRigidBody = GetComponent<Rigidbody>();
    }

    private void OnMove()
    {
        Destroy(gameObject);
    }
}
