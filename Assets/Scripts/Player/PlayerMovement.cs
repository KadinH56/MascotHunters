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
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerStatManager statManager;

    //[SerializeField] private float playerSpeed = 8f;
    [SerializeField] private float dashSpeed = 5f;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] private bool isRoll;

    private Rigidbody pRigidBody;
    private PlayerInput pInput;

    private InputAction move;
    private InputAction roll;

    //Used to ID the player for local multiplayer. Set to 0 or 1 ingame
    private int playerID = -1;
    private float rollTimer;

    private Vector3 moveDir;
    private Vector3 facing = Vector3.right;

    private CameraFollower cam;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private AudioClip rollSound;
    [SerializeField] private AudioClip moveSound;

    [SerializeField] private WeaponManager weaponManager;

    [SerializeField] private RuntimeAnimatorController player1;
    [SerializeField] private RuntimeAnimatorController player2;

    /// <summary>
    /// Used by the Player Manager to set the player ID to 0 or 1
    /// </summary>
    public int PlayerID { get => playerID; set => playerID = value; }
    public Vector3 Facing { get => facing; set => facing = value; }
    public WeaponManager WeaponManager { get => weaponManager; set => weaponManager = value; }

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
        cam = FindFirstObjectByType<CameraFollower>();

        //weaponManager = GetComponent<WeaponManager>();

        //animator = GetComponent<Animator>();

    }

    public void SetSprite()
    {
        animator.runtimeAnimatorController = playerID == 0 ? player1 : player2;
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

        if(rollTimer <= 0)
        {
            isRoll = true;
            AudioSource.PlayClipAtPoint(rollSound, transform.position);
            //Vector3 moveDir = new(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y);
            //pRigidBody.AddForce(new Vector2(moveDir.x * dashSpeed, moveDir.y * dashSpeed));
            moveDir *= dashSpeed;

            //Starts the coroutine
            StartCoroutine(Roll());
        }
        else
        {
            Debug.Log("Cannot roll right now!");
        }
    }

    private IEnumerator Roll()
    {
        if(isRoll == true)
        {
            animator.SetBool("IsRolling", true);
            yield return new WaitForSeconds(dashTime);
            animator.SetBool("IsRolling", false);
            isRoll = false;
            rollTimer = 1.25f;
        }
    }

    /// <summary>
    /// The best player movement ever seen
    /// </summary>
    private void FixedUpdate()
    {
        if (rollTimer > 0)
        {
            rollTimer -= Time.deltaTime;
        }

        if (!isRoll)
        {
            moveDir = new(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y);
        }

        if(moveDir != Vector3.zero)
        {
            facing = moveDir;
        }

        pRigidBody.linearVelocity = Vector3.MoveTowards(pRigidBody.linearVelocity, moveDir * statManager.PlayerStats.Movement, 1f);
        
        //if (Vector2.Distance(new(transform.position.x, transform.position.z), new(cam.transform.position.x - cam.OffSet.x, cam.transform.position.z - cam.OffSet.z)) > cam.MaxCameraDistance)
        //{
        //    //Vector3 position = (cam.transform.position - cam.OffSet) + transform.position;
        //    //position.Normalize();
        //    //transform.position = position * cam.MaxCameraDistance;

        //    Vector2 pPos = new(transform.position.x, transform.position.z);
        //    Vector2 cPos = new(cam.transform.position.x - cam.OffSet.x, cam.transform.position.z - cam.OffSet.z);

        //    Vector2 direction = cPos + pPos;
        //    direction.Normalize();

        //    direction *= cam.MaxCameraDistance;
        //    print(direction);
        //    transform.position = new(direction.x, 1.5f, direction.y);
        //}

        if(moveDir == Vector3.zero)
        {
            animator.SetBool("IsMoving", false);
            return;
        }

        animator.SetBool("IsMoving", !isRoll);

        if (facing.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    
        Vector3 average = Vector3.zero;
        float size = 0f;
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!player.activeSelf)
            {
                continue;
            }

            average += player.transform.position;
            size += 1f;
        }        

        average /= size;

        if(Vector3.Distance(transform.position, average) > cam.MaxCameraDistance)
        {
            Vector3 direction = transform.position - average;
            direction.Normalize();
            direction *= cam.MaxCameraDistance;
            transform.position = average + direction;
        }

    }
}
