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
    private PlayerInput pInput;
    private Rigidbody pRigidBody;

    private InputAction move;
    private InputAction special;

    /// <summary>
    /// This is for local multiplayer. The arcade cabinet could support up to 2 players simeotaneously
    /// </summary>
    [SerializeField] private int player = 0;

    /// <summary>
    /// Sets some private variables and starts the action map
    /// Handled in PMovement because this is garunteed to be active upon loading the scene
    /// </summary>
    public void Awake()
    {
        //Init above variables
        pInput = GetComponent<PlayerInput>();
        pRigidBody = GetComponent<Rigidbody>();

        //Start the action map
        pInput.currentActionMap.Enable();
    }
}
