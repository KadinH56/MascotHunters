/*****************************************************************************
// File Name : PlayerMovement
// Author : Lucas Fehlberg
// Creation Date : September 2, 2025
// Last Updated : September 2, 2025
//
// Brief Description : Player movement. Also turns on the action map and handles mapping to player movement and some
//  Special abilities, including dodge roll
*****************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerStatManager statManager;

    [SerializeField] private float playerSpeed = 8f;
    [SerializeField] private float dashSpeed = 5f;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] private bool isRoll;

    private Rigidbody pRigidBody;
    private PlayerInput pInput;

    private InputAction move;
    private InputAction roll;

    //Used to ID the player for local multiplayer. Set to 0 or 1 ingame
    private int playerID = -1;

    private Vector3 moveDir;

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
        pInput = GetComponent<PlayerInput>();
        statManager = GetComponent<PlayerStatManager>();

        move = pInput.currentActionMap.FindAction("Move");
        roll = pInput.currentActionMap.FindAction("Roll");

        //Creates the function when the button for the roll is pressed
        roll.started += Roll_started;
    }

    /// <summary>
    /// Controls what happens when the player hits the dash button
    /// </summary>
    /// <param name="obj"></param>
    private void Roll_started(InputAction.CallbackContext obj)
    {
        if(moveDir == Vector3.zero)
        {
            return;
        }
        isRoll = true;
        Debug.Log("Roll started!");

        //Vector3 moveDir = new(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y);
        //pRigidBody.AddForce(new Vector2(moveDir.x * dashSpeed, moveDir.y * dashSpeed));
        moveDir *= dashSpeed;

        //Starts the coroutine
        StartCoroutine(Roll());
    }

    private IEnumerator Roll()
    {
        yield return new WaitForSeconds(dashTime);

        Debug.Log("Coroutine ended!");
        isRoll = false;
    }

    /// <summary>
    /// The best player movement ever seen
    /// </summary>
    private void FixedUpdate()
    {
        if (!isRoll)
        {
            moveDir = new(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y);
        }

        pRigidBody.linearVelocity = Vector3.MoveTowards(pRigidBody.linearVelocity, moveDir * statManager.PlayerStats.Movement, 1f);
    }
}
