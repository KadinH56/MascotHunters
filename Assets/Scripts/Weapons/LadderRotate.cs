using UnityEngine;
using UnityEngine.UIElements;

public class LadderRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform rotateAround;
    [SerializeField] private bool isRotating;

    private PlayerMovement playerMovement;

    void Start()
    {
        
    }

    /// <summary>
    /// If the bool is true, the ladder will rotate around the fixed object
    /// </summary>
    void FixedUpdate()
    {
        if(isRotating == true)
        {
            this.transform.RotateAround(playerMovement.playerPosition, Vector3.down, rotationSpeed * Time.deltaTime);
        }
    }
}
